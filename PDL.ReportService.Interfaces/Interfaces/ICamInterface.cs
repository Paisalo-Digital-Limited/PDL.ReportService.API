using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PDL.ReportService.Interfaces.Interfaces
{
    public interface ICamInterface
    {
        string GetCamGeneration(string ficodes, string creator, string dbName, bool isLive);
        List<string> GetFiCodeByCreator(int CreatorId, string dbName, bool isLive);
    }
}
