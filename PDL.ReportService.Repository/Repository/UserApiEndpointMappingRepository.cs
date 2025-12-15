using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.Credentials;
using PDL.ReportService.Logics.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PDL.ReportService.Repository.Repository
{
    public class UserApiEndpointMappingRepository : IUserApiEndpointMappingRepository
    {
        private readonly CredManager _credManager;
        private readonly IConfiguration _configuration;

        public UserApiEndpointMappingRepository(CredManager credManager, IConfiguration configuration)
        {
            _credManager = credManager;
            _configuration = configuration;
        }

        protected bool GetIslive()
        {
            var value = _configuration["isliveDb"];
            return bool.TryParse(value, out var result) && result;
        }

        public async Task<bool> IsUserMappedToEndpointAsync(int userId, string controllerName, string functionName, string serviceName)
        {
            string dbname = Helper.GetDBName(_configuration);




            using (SqlConnection conn = _credManager.getConnections(dbname, GetIslive()))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SP_CheckUserEndpointMapping", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                    cmd.Parameters.Add("@ControllerName", SqlDbType.NVarChar, 200).Value = controllerName;
                    cmd.Parameters.Add("@FunctionName", SqlDbType.NVarChar, 200).Value = functionName;
                    cmd.Parameters.Add("@ServiceName", SqlDbType.NVarChar, 200).Value = serviceName;

                    var result = await cmd.ExecuteScalarAsync();

                    return Convert.ToInt32(result) == 1;
                }
            }
        }
    }
}
