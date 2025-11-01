using DocumentFormat.OpenXml.EMMA;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Logics.Credentials;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Logics.BLL
{
    public class AllReportsBLL:BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;

        public AllReportsBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }
        public DataTable AllReportsList(AllReportParameterVM model, string dbname, bool isLive)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = _credManager.getConnections(dbname, isLive))
            {
                using (var cmd = new SqlCommand("Usp_GetAllReportsList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "AllReportsList");
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@ReportName", (object?)model.ReportName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@DatabaseName", (object?)model.DatabaseName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Creator", (object?)model.Creator ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BranchCodeFrom", (object?)model.BranchCodeFrom ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BranchCodeTo", (object?)model.BranchCodeTo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FromDate", (object?)model.FromDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate", (object?)model.ToDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SchemeType", (object?)model.SchemeType ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FO1", (object?)model.FO1 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FO2", (object?)model.FO2 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToCode", (object?)model.ToCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Area", (object?)model.Area ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tag", (object?)model.Tag ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FICode", (object?)model.FICode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Code", (object?)model.Code ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ahead", (object?)model.Ahead ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", (object?)model.Name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FinAmount", (object?)model.FinAmount ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FromCode", (object?)model.FromCode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Trench", (object?)model.Trench ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MobNo", (object?)model.MobNo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Endpoints", (object?)model.Endpoints ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@ModuleType", (object?)model.ModuleType ?? DBNull.Value);
                    //cmd.Parameters.AddWithValue("@Mode", (object?)model.Mode ?? DBNull.Value);
                    con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        public DataTable RcPostReportsList(int CreatorID, string? VDate, string? VNO, string? FromDate, string? ToDate, int? PageSize, int? PageNumber, string dbname, bool isLive)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con= _credManager.getConnections(dbname, isLive))
            {
                using(SqlCommand cmd=new SqlCommand("Usp_GetAllReportsList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "RcPostReportsList");
                    cmd.Parameters.AddWithValue("@CreatorID,", CreatorID);
                    cmd.Parameters.AddWithValue("@VDate", VDate);
                    cmd.Parameters.AddWithValue("@VNO", VNO);
                    cmd.Parameters.AddWithValue("@FromDate", FromDate);
                    cmd.Parameters.AddWithValue("@ToDate", ToDate);
                    cmd.Parameters.AddWithValue("@PageSize", PageSize);
                    cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
                    con.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
