using Microsoft.Extensions.Configuration;
using PDL.Authentication.Entites.VM;
using PDL.FIService.Logics.BLL;
using PDL.ReportService.Logics.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Repository.Repository
{
    public class UserRepository : BaseBLL
    {
        private readonly IConfiguration _configuration;


        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task<AccountTokens> GetByIdAsync(long userId)
        {

            using (UserBLL userBLL = new UserBLL(_configuration))
            {
                return await userBLL.GetByIdAsync(userId);
            }
        }

    }
}
