using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PDL.FIService.Api.HealthCheck
{
    public class SqlConnectionHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private SqlConnection sqlConnection;

        public SqlConnectionHealthCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnectionHealthCheck(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync(cancellationToken);

                using var command = new SqlCommand("SELECT 1", connection);
                await command.ExecuteScalarAsync(cancellationToken);

                return HealthCheckResult.Healthy("SQL connection is healthy");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("SQL connection failed", ex);
            }
        }
    }
}
