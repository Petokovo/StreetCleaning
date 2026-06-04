using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace StreetCleaning.Data.Interceptors
{
    public class OracleNlsInterceptor : DbConnectionInterceptor
    {
        // This interceptor is triggered after an Oracle DB connection is successfully opened.
        // It allows us to run custom session-level ALTER SESSION commands automatically
        // for every EF Core connection.

        public override void ConnectionOpened(DbConnection c, ConnectionEndEventData e)
        {
            // Create a command object bound to the current open connection
            using var cmd = c.CreateCommand();

            // Enable linguistic comparison instead of binary comparison.
            // This allows Oracle to compare strings using linguistic rules
            // (e.g., proper handling of accented characters, case-insensitive comparisons
            // when used together with BINARY_AI sort).
            cmd.CommandText = "ALTER SESSION SET NLS_COMP=LINGUISTIC";
            cmd.ExecuteNonQuery();

            // Set the sort order to BINARY_AI:
            // - BINARY = binary sort
            // - A = accent-insensitive
            // - I = case-insensitive
            // Together with NLS_COMP=LINGUISTIC, this makes LIKE and = comparisons
            // accent- and case-insensitive at the session level.
            cmd.CommandText = "ALTER SESSION SET NLS_SORT=BINARY_AI";
            cmd.ExecuteNonQuery();
        }
    }
}
