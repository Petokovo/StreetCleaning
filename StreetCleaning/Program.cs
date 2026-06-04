using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using Serilog.Events;
using System.IO.Compression;
using System.Threading.RateLimiting;

namespace StreetCleaning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                var appPath = builder.Configuration["SerilogFiles:AppPath"];
                var accessPath = builder.Configuration["SerilogFiles:AccessPath"];
                var retainAppDays = builder.Configuration.GetValue<int>("SerilogFiles:RetainAppDays");
                var retainAccessDays = builder.Configuration.GetValue<int>("SerilogFiles:RetainAccessDays");

                if (appPath == null || accessPath == null)
                {
                    throw new Exception("Serilog file configuration is missing.");
                }
                builder.Logging.ClearProviders();

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)

                    // 1) APP log
                    .WriteTo.Logger(lc => lc
                        .Filter.ByExcluding(e => e.Properties.ContainsKey("AccessLog"))
                        .WriteTo.File(
                            path: appPath,
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: retainAppDays,
                            outputTemplate: "{Timestamp:yyyy-MM-ddTHH:mm:ss.fffffffzzz}\t{Level:u3}\t{SourceContext}\t{Message:lj}{NewLine}"
                        )
                    )

                    // 2) ACCESS log
                    .WriteTo.Logger(lc => lc
                        .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("AccessLog"))
                        .WriteTo.File(
                            path: accessPath,
                            rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: retainAccessDays,
                            outputTemplate: "{Timestamp:yyyy-MM-ddTHH:mm:ss.fffffffzzz}\t{Message:lj}{NewLine}"
                        )
                    ).CreateLogger();

                builder.Host.UseSerilog(Log.Logger, dispose: true);

                builder.Services.AddScoped<IPlnOdsNotifyRepository, PlnOdsNotifyRepository>();
                builder.Services.AddScoped<IPlnOdsService, PlnOdsService>();
                builder.Services.AddSingleton<IAppVersionService, AppVersionService>();

                builder.Services.AddDbContext<PlnOdsDbContext>(options =>
                    options.UseOracle(builder.Configuration.GetConnectionString("DbConnectionString"))
                    .AddInterceptors(new OracleNlsInterceptor())
                    .AddInterceptors(new OracleFetchTuningInterceptor()));

                // Add services to the container.
                builder.Services.AddControllersWithViews();

                builder.Services.AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });

                builder.Services.Configure<BrotliCompressionProviderOptions>(o =>
                {
                    o.Level = CompressionLevel.Fastest;
                });

                builder.Services.Configure<GzipCompressionProviderOptions>(o =>
                {
                    o.Level = CompressionLevel.Fastest;
                });

                builder.Services.AddRateLimiter(options =>
                {
                    // Per-IP token bucket – activated with [EnableRateLimiting("PerIpBurstPolicy")]
                    options.AddPolicy("PerIpBurstPolicy", httpContext =>
                    {
                        var key = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                        return RateLimitPartition.GetTokenBucketLimiter(key, _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = 3,                                 // Maximum number of tokens (burst capacity).
                            TokensPerPeriod = 1,                            // How many tokens are added each period.
                            ReplenishmentPeriod = TimeSpan.FromSeconds(1),  // Period length (1 second).
                            AutoReplenishment = true,                       // Tokens are automatically replenished on schedule.
                            QueueLimit = 2,                                 // Max number of requests waiting in the queue per IP.
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst // FIFO order in the queue.
                        });
                    });

                    // Global fixed-window limiter – applies automatically to the whole app
                    // Limits the total number of requests across all clients.
                    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
                        RateLimitPartition.GetFixedWindowLimiter("GLOBAL", _ => new FixedWindowRateLimiterOptions
                        {
                            Window = TimeSpan.FromSeconds(1),   // Length of the fixed time window.
                            PermitLimit = 30,                   // Max number of allowed requests per window.
                            QueueLimit = 30,                    // Max number of requests waiting in the global queue.
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst // FIFO for global queue.
                        }));

                    // What happens when a request is rejected
                    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // HTTP status code to return.
                    options.OnRejected = async (ctx, token) =>
                    {
                        ctx.HttpContext.Response.Headers.RetryAfter = "1"; // Tell client to retry after 1 second.
                        await ctx.HttpContext.Response.WriteAsync("Too many requests.", token); // Response body.
                    };
                });

                var app = builder.Build();

                //Forwarded headers
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });

                if (builder.Configuration.GetValue<bool>("AccessLog:Enabled"))
                {
                    app.Use(async (ctx, next) =>
                    {
                        var sw = System.Diagnostics.Stopwatch.StartNew();
                        await next();
                        sw.Stop();

                        var xff = ctx.Request.Headers["X-Forwarded-For"].ToString();
                        var clientIp = !string.IsNullOrWhiteSpace(xff)
                            ? xff.Split(',')[0].Trim()
                            : ctx.Connection.RemoteIpAddress?.ToString() ?? "";

                        var ua = ctx.Request.Headers.UserAgent.ToString();
                        var qs = ctx.Request.QueryString.HasValue ? ctx.Request.QueryString.Value : "";

                        Log.ForContext("AccessLog", true)
                           .Information("{IP}, {Method}, {Path}, {QS} , {Status} in {Ms}ms, UA={UA}",
                               clientIp,
                               ctx.Request.Method,
                               ctx.Request.Path,
                               qs,
                               ctx.Response.StatusCode,
                               sw.ElapsedMilliseconds,
                               ua);
                    });
                }

                var pathBase = app.Configuration["PathBase"];
                if (!string.IsNullOrWhiteSpace(pathBase))
                {
                    app.UsePathBase(pathBase);
                }

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }
                app.UseResponseCompression();

                app.UseHttpsRedirection();

                app.UseStaticFiles();

                app.UseRouting();

                app.UseRateLimiter();

                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal("Application start-up failed: " + ex.ToString());
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
