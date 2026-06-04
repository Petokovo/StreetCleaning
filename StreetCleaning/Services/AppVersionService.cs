using StreetCleaning.Services.Interfaces;
using System.Reflection;

namespace StreetCleaning.Services
{
    public class AppVersionService : IAppVersionService
    {
        public string GetCurrentVersion()
        {
            var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

            var info = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            var version = !string.IsNullOrWhiteSpace(info)
                ? info
                : (asm.GetName().Version?.ToString() ?? "unknown");

            var plusIndex = version.IndexOf('+');
            if (plusIndex >= 0)
                version = version.Substring(0, plusIndex);

            return version;
        }
    }
}
