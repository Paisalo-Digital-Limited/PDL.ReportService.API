using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Logics.Credentials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Repository.Repository
{
    public class HelperRepository
    {
        private IConfiguration _configuration;

        public HelperRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string crifdata(string cr, string fi)
        {
            DataSet dsfi = new DataSet();
            string res = string.Empty;
            using (var con = new SqlConnection(_configuration.GetConnectionString("PaisaloConnection")))
            {
                try
                {
                    string com = string.Empty;

                    //com = " Select Base64XmlDoc from crifreport ";
                    //com += " where Creator = '" + cr + "' And FiCode = '" + fi + "' order by CreationDate desc";
                    com = "Usp_GetCamGeneration";

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand(com, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GETDATACRIFREPORT");
                    cmd.Parameters.Add("@Creator", SqlDbType.VarChar).Value = cr;
                    cmd.Parameters.Add("@FiCode", SqlDbType.VarChar).Value = fi;
                    //SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dsfi);
                    con.Close();
                    res = Convert.ToString(dsfi.Tables[0].Rows[0][0]);
                }
                catch (Exception ex)
                {

                }
            }
            return res;
        }

        public string GetJsonCrif(string cr, string fi, CredManager _credManager, string dbName, bool islive)
        {
            DataTable dsfi = new DataTable();
            using (SqlConnection con = _credManager.getConnections(dbName, islive))
            //using (var con = new SqlConnection(_configuration.GetConnectionString("PaisaloConnection")))
            {
                try
                {
                    string com = string.Empty;

                    //com = "SELECT JsonData FROM CrifScoreDealers WHERE Creator='" + cr + "' AND FICode='" + fi + "' order by CreatedOn desc";
                    com = "Usp_GetCamGeneration";

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand(com, con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetJsonCrifData");
                    cmd.Parameters.Add("@Creator", SqlDbType.VarChar).Value = cr;
                    cmd.Parameters.Add("@FiCode", SqlDbType.VarChar).Value = fi;
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    //SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                    adpt.Fill(dsfi);
                    con.Close();
                }
                catch (Exception ex)
                {

                }
            }
            return dsfi.Rows.Count > 0 ? dsfi.Rows[0]["JsonData"].ToString() : string.Empty;
        }

        public DataTable RemoveDuplicatesFromDataTable(DataTable table, List<string> keyColumns)
        {
            Dictionary<string, string> uniquenessDict = new Dictionary<string, string>(table.Rows.Count);
            StringBuilder stringBuilder = null;
            int rowIndex = 0;
            DataRow row;
            DataRowCollection rows = table.Rows;
            while (rowIndex < rows.Count - 1)
            {
                row = rows[rowIndex];
                stringBuilder = new StringBuilder();
                foreach (string colname in keyColumns)
                {
                    //stringBuilder.Append(((double)row[colname]));
                    stringBuilder.Append(row[colname]);
                }
                if (uniquenessDict.ContainsKey(stringBuilder.ToString()))
                {
                    rows.Remove(row);
                }
                else
                {
                    uniquenessDict.Add(stringBuilder.ToString(), string.Empty);
                    rowIndex++;
                }
            }

            return table;
        }

        public string getemi(string bank, int dur)
        {
            DataSet dsgc = new DataSet();
            string emi = string.Empty;
            using (var con = new SqlConnection(_configuration.GetConnectionString("PaisaloConnection")))
            {
                try
                {
                    string com = "	Select Emi from CoLendingEmi  " +
                        " where Bank='" + bank + "' ";// and fs.schcode like 'BB%' and";
                    com += " and Duration = '" + dur + "' ";// and fi.Code not in(Select FiCode from[BankFIApproval] where FiLoanAc is null)";


                    SqlDataAdapter adpt = new SqlDataAdapter(com, con);
                    adpt.Fill(dsgc);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            emi = dsgc.Tables[0].Rows.Count > 0 ? Convert.ToString(dsgc.Tables[0].Rows[0][0]) : "";
            return emi;
        }
        public DataTable getBREdata(string Bank, int duration)
        {
            DataTable dscam = new DataTable();

            string sql = "getBREMatrixs";
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("PaisaloConnection")))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Bank", Bank.Trim());
                cmd.Parameters.AddWithValue("@Duration", duration);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dscam);
            }
            return dscam;
        }
    }
}
