using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using PDL.ReportService.Entites.VM.ReportVM;
using PDL.ReportService.Logics.Credentials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PDL.ReportService.Logics.BLL
{
    public class ReportsBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;

        public ReportsBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }

        public List<CaseHistoryVM> GetCaseHistoryBySmCodes(List<string> smCodes, string dbName, bool isLive)
        {
            List<CaseHistoryVM> result = new List<CaseHistoryVM>();

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                con.Open();

                foreach (var smCode in smCodes)
                {
                    using (SqlCommand cmd = new SqlCommand("usp_GetCaseHistoryBySmCode", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SmCode", smCode);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                result.Add(new CaseHistoryVM
                                {
                                    Code = reader["Code"]?.ToString(),
                                    Custname = reader["Custname"]?.ToString(),
                                    Religion = reader["Religion"]?.ToString(),
                                    PhoneNo = reader["PhoneNo"]?.ToString(),
                                    Income = reader["Income"] as decimal?,
                                    CrifScore = reader["CrifScore"] as int?,
                                    OverdueAmt = reader["OverdueAmt"] as decimal?,
                                    TotalCurrentAmt = reader["TotalCurrentAmt"] as decimal?,
                                    Pincode = reader["Pincode"]?.ToString(),
                                    Address = reader["Address"]?.ToString(),
                                    Creator = reader["Creator"]?.ToString(),
                                    IncomePA = reader["IncomePA"] as decimal?
                                });
                            }
                        }
                    }
                }

                con.Close();
            }

            return result;
        }
    }
}
