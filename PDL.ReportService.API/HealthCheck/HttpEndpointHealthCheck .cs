using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PDL.FIService.Api.HealthCheck
{
    public class HttpEndpointHealthCheck : IHealthCheck
    {
        private readonly string _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpEndpointHealthCheck(string url, IHttpClientFactory httpClientFactory)
        {
            _url = url;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(_url, cancellationToken);

                if (response.IsSuccessStatusCode)
                    return HealthCheckResult.Healthy($"Pod at {_url} is healthy");

                return HealthCheckResult.Unhealthy($"Pod at {_url} returned status {response.StatusCode}");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Pod at {_url} is unreachable", ex);
            }
        }
    }
}
