using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Credentials;
using PDL.ReportService.Repository.Repository;

namespace PDL.ReportService.API.Extensions
{
    public static class ServiceExtensions
    {
        #region Dependency Inject
        public static void RegisterRepository(this IServiceCollection collection)
        {
            collection.AddMemoryCache();
            collection.AddHttpClient();
            collection.AddScoped<CredManager>();
            collection.AddScoped<IBranchDashboardService, BranchDashboardRepository>();
            collection.AddScoped<IReports, ReportsRepository>();
            collection.AddScoped<ICamInterface, CamRepository>();
            collection.AddScoped<IAllReportsService, AllReportsRepository>();
            collection.AddScoped<UserRepository>();
        }
        #endregion
    }
}
