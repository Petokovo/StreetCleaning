using Microsoft.EntityFrameworkCore.Diagnostics;
using Oracle.ManagedDataAccess.Client;

namespace StreetCleaning.Data.Interceptors
{
    public class OracleFetchTuningInterceptor : DbCommandInterceptor
    {
        // Tuning parameters – adjust based on average row size
        private const int FetchSizeBytes = 256 * 1024;
        private const int InitialLobFetchSize = -1;

        public override InterceptionResult<System.Data.Common.DbDataReader> ReaderExecuting(
            System.Data.Common.DbCommand command,
            CommandEventData eventData,
            InterceptionResult<System.Data.Common.DbDataReader> result)
        {
            if (command is OracleCommand oc)
            {
                // how many bytes are fetched in a single batch
                oc.FetchSize = FetchSizeBytes;

                // fetch the entire LOB at once
                oc.InitialLOBFetchSize = InitialLobFetchSize;

                // Performance and plan reuse
                oc.AddToStatementCache = true;
                oc.BindByName = true;
            }

            return base.ReaderExecuting(command, eventData, result);
        }
    }
}
