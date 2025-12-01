using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.ReportService.Logics.BLL;
using PDL.ReportService.Logics.Credentials;
using PDL.ReportService.Logics.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.FIService.Logics.BLL
{
    public class UserBLL:BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;

        private readonly HttpClient _httpClient;

        public UserBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);

            _httpClient = new HttpClient();

        }

        public async Task<AccountTokens> GetByIdAsync(long userId)
        {
            bool isLive = GetIslive();
            string dbName = GetDBName();
            AccountTokens user = null;
            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                string query = "Usp_UserDetails";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@userId", SqlDbType.BigInt).Value = userId;
                    cmd.Parameters.Add("@Mode", SqlDbType.VarChar).Value = "GETUSER";
                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new AccountTokens
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                TokenVersion = reader.GetInt32(reader.GetOrdinal("TokenVersion"))
                            };
                        }
                    }
                }
            }


            return user;
        }
        protected string GetDBName()
        {
            string val = _configuration["encryptSalts:dbval"];
            string salt = _configuration["encryptSalts:dbName"];
            val = Helper.Decrypt(val, salt);
            return val;
        }
        protected bool GetIslive()
        {
            bool val = false;
            val = Convert.ToBoolean(_configuration["isliveDb"]);
            return val;
        }


    }
}
