using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Interfaces.Interfaces
{
    public interface IUserApiEndpointMappingRepository
    {
        Task<bool> IsUserMappedToEndpointAsync(int userId, string controllerName, string functionName, string serviceName);
    }
}
