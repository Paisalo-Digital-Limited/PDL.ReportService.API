using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Interfaces.Interfaces;
using PDL.ReportService.Logics.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Repository.Repository
{
    public class CamRepository : BaseBLL, ICamInterface
    {
        private IConfiguration _configuration;

        public CamRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GetCamGeneration(string ficodes, string creatorId, string dbName, bool isLive)

        {
            using (CamBLL camBLL = new CamBLL(_configuration))
            {
                return camBLL.GetCamGeneration(ficodes, creatorId, dbName, isLive);

            }
        }
        public List<string> GetFiCodeByCreator(int CreatorId, string dbName, bool isLive)
        {
            using (CamBLL camBLL = new CamBLL(_configuration))
            {
                return camBLL.GetFiCodeByCreator(CreatorId, dbName, isLive);
            }
        }
    }
}
