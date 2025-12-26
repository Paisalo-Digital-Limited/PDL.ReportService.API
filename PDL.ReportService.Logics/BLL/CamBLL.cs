using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PDL.ReportService.Entites.VM;
using PDL.ReportService.Logics.BLL;
using PDL.ReportService.Logics.Credentials;
using PDL.ReportService.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace PDL.ReportService.Logics.BLL
{
    public class CamBLL : BaseBLL
    {
        private readonly IConfiguration _configuration;
        private readonly CredManager _credManager;

        public CamBLL(IConfiguration configuration)
        {
            _configuration = configuration;
            _credManager = new CredManager(configuration);
        }


        public string GetCamGeneration(string ficodes, int creatorId, string dbName, bool isLive)
        {
            String selcam = string.Empty;


            var ficodesArray = ficodes.Replace("'", "").Split(',');
            var formattedFiCodes = string.Join(",", ficodesArray);

            selcam = @"Usp_GetCamGeneration";

            DataSet dsc = new DataSet();
            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                using (var cmd = new SqlCommand(selcam, con))
                {
                    var da = new SqlDataAdapter(cmd);
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    //cmd.CommandType = CommandType.text;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GETCAMDATA");
                    cmd.Parameters.Add("@FiCode", SqlDbType.VarChar).Value = formattedFiCodes;
                    cmd.Parameters.Add("@Creator", SqlDbType.Int).Value = creatorId;
                    da.Fill(dsc);
                    con.Close();
                }
            }
            if (dsc.Tables.Count > 0)
            {
                CrifWithFIVM obj = new CrifWithFIVM();
                dynamic tii = null;
                dynamic existingemii = null;
                dynamic totexpp = null;

                HelperRepository helper = new HelperRepository(_configuration);
                List<string> columnName = new List<string> { "Code", "Creator" };
                DataTable dataGrid = helper.RemoveDuplicatesFromDataTable(dsc.Tables[0], columnName);
                dataGrid.Columns.Add("LoanApproved");
                dataGrid.Columns.Add("80");
                dataGrid.Columns.Add("20");

                DataTable dtcam = new DataTable();
                int totexp = 0;
                int ti = 0;
                string emibase = "0";
                string existingemi = "0";
                string creator = dataGrid.Rows[0]["Creator"].ToString();
                if (dataGrid != null && dataGrid.Rows.Count > 0)
                {
                    string camhtml = string.Empty;
                    camhtml += "<div class='container'>";
                    camhtml += "<div style=' background: #e5dfdf;  padding: 5px 0px; margin-top: 10px;'>";
                    camhtml += "<h5 class='text-center'style='margin-top:30px margin-bottom:30px'>CLIENT APPRAISAL MEMORANDUM (In Duplicat)</h5>";
                    camhtml += "<p class='text-center'>Co-lending for BANK-PDL loan Rs 2 lakhs</p></div>";
                    camhtml += "<div class='row mt-3 mb-3'>";
                    camhtml += "<div class='col-md-4'>";

                    camhtml += " <span>DATE OF CAM</span>";
                    camhtml += " <span>" + System.DateTime.Now.ToString("dd-MM-yyyy") + "</span><br>";
                    camhtml += " <span>CREATOR</span>";
                    camhtml += " <span><b>" + creator.ToUpper().Trim() + "</b></span>";
                    camhtml += "<br>";
                    camhtml += " <span>GROUP NO.</span>";
                    camhtml += " <span><b>" + dataGrid.Rows[0]["Branch_Code"].ToString().ToUpper() + "</b></span>";
                    camhtml += "</div>";
                    camhtml += "<div class='col-md-4'>";
                    camhtml += " <span>BRANCH NAME</span>";
                    camhtml += " <span><b>" + dataGrid.Rows[0]["branchname"].ToString().ToUpper() + " </b></span><br>";
                    camhtml += " <span>BRANCH CODE</span>";
                    camhtml += " <span>" + dataGrid.Rows[0]["Branch_Code"].ToString().ToUpper() + "</span>";
                    camhtml += " <br>";
                    camhtml += " <span>Disbursement Date</span>";
                    camhtml += " <span></span>";
                    camhtml += "<br>";
                    camhtml += " <span>DE DUPE</span>";
                    camhtml += " <span>DONE</span>";
                    camhtml += "</div>";
                    camhtml += "</div>";
                    camhtml += "<div class='table-responsive'>";
                    camhtml += "<table style='width:100%'>";

                    camhtml += "<tr>";
                    camhtml += "  <td style='padding-left: 10px;' >S.<br>No</td>";
                    camhtml += "  <td colspan='4'>BORROWER'S NAME (AS PER PAN CARD IF ANY)</td>";
                    camhtml += "  <td style='text-align: center;'>FI NO.</td>";
                    camhtml += "  <td style='text-align: center;'>DOB APP</td>";
                    camhtml += "   <td style='text-align: center;'>DOB Nominee</td>";
                    camhtml += "   <td style='text-align: center;'>Loan Applied</td>";
                    camhtml += "   <td style='text-align: center;'>Tenure (months)</td>";
                    camhtml += "   <td style='text-align: center;'>Loan Approved Amt.</td>";
                    camhtml += "   <td colspan='8' style='text-align: center;'>Sanction/Rejection Remarks</td>";
                    camhtml += "</tr>";
                    int rowc1 = 0;
                    string ScoreValue = "";
                    for (var irow = 0; irow < dataGrid.Rows.Count; irow++)
                    {
                        rowc1++;
                        camhtml += "<tr>";
                        camhtml += "  <td style='padding-left: 10px;' >" + rowc1 + "</td>";
                        camhtml += "  <td colspan='4'>" + dataGrid.Rows[irow]["custname"].ToString().ToUpper() + " " + dsc.Tables[0].Rows[irow]["Gaurdian"].ToString().ToUpper() + "</td>";
                        camhtml += "  <td style='text-align: center;'>" + dataGrid.Rows[irow]["Code"].ToString().ToUpper() + "</td>";
                        camhtml += "  <td style='text-align: center;'>" + dataGrid.Rows[irow]["DOB"].ToString().ToUpper() + "</td>";
                        camhtml += "   <td style='text-align: center;'>" + Convert.ToString(dataGrid.Rows[irow]["DobNominee"]) + "</td>";
                        camhtml += "   <td style='text-align: center;'>" + dataGrid.Rows[irow]["Loan_amount"].ToString().ToUpper() + "</td>";
                        camhtml += "   <td style='text-align: center;'>" + dataGrid.Rows[irow]["Loan_Duration"].ToString().ToUpper() + "</td>";
                        /////////////////////
                        //  camhtml += "</tr>";

                        string los = string.Empty;
                        string node = "0";
                        string node1 = "0";
                        string node2 = "0";
                        string nodeemi = "0";
                        string PaymentHistory = "";
                        string OverdueAmt = "";

                        string Writeoff = string.Empty;
                        int Overduetotal = 0;
                        int Writetotal = 0;
                        int finaldpd = 0;
                        string getcrifxml = helper.crifdata(creator.ToUpper().Trim(), dataGrid.Rows[irow]["Code"].ToString().ToUpper());
                        string jsonCrif = helper.GetJsonCrif(creator.ToUpper().Trim(), dataGrid.Rows[irow]["Code"].ToString(), _credManager, dbName, isLive);
                        //if (!string.IsNullOrEmpty(getcrifxml))
                        //{
                        //    string xmlrep = Encoding.UTF8.GetString(Convert.FromBase64String(getcrifxml));

                        //    XmlDocument xmlDoc = new XmlDocument();
                        //    xmlDoc.LoadXml(xmlrep);


                        //    XmlNodeList xnListBal11 = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/RESPONSES/RESPONSE/LOAN-DETAILS/COMBINED-PAYMENT-HISTORY");
                        //    if (xnListBal11 != null && xnListBal11.Count > 0)
                        //    {
                        //        foreach (XmlNode xn in xnListBal11)
                        //        {
                        //            var finalsepa = xn.InnerText.Split('|');
                        //            if (finalsepa.Length > 0)
                        //            {
                        //                for (int j = 0; j < finalsepa.Length; j++)
                        //                {
                        //                    bool isDateCheck = CheckEmiDate(finalsepa[j]);
                        //                    if (isDateCheck)
                        //                    {
                        //                        finaldpd = finaldpd + Convert.ToInt32(!string.IsNullOrEmpty(finalsepa[j].ToString()) ? Regex.IsMatch(finalsepa[j].ToString().Substring(finalsepa[j].ToString().IndexOf(",") + 1, 3), @"^\d+$") ? finalsepa[j].ToString().Substring(finalsepa[j].ToString().IndexOf(",") + 1, 3) : 0 : 0);
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }

                        //    XmlNodeList xnListinstall = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/INDV-RESPONSES/PRIMARY-SUMMARY/TOTAL-OTHER-CURRENT-BALANCE");

                        //    if (xnListinstall != null && xnListinstall.Count > 0)
                        //    {
                        //        foreach (XmlNode xn in xnListinstall)
                        //        {
                        //            node = xn.InnerText;

                        //        }
                        //    }

                        //    if (node == "0")
                        //    {

                        //        XmlNodeList xnListinstall1 = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/ACCOUNTS-SUMMARY/PRIMARY-ACCOUNTS-SUMMARY/PRIMARY-CURRENT-BALANCE");
                        //        if (xnListinstall1 != null && xnListinstall1.Count > 0)
                        //        {
                        //            foreach (XmlNode xn in xnListinstall1)
                        //            {
                        //                node1 = xn.InnerText;

                        //            }
                        //        }

                        //        XmlNodeList xnListinstall2 = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/ACCOUNTS-SUMMARY/SECONDARY-ACCOUNTS-SUMMARY/SECONDARY-CURRENT-BALANCE");
                        //        if (xnListinstall2 != null && xnListinstall2.Count > 0)
                        //        {
                        //            foreach (XmlNode xn in xnListinstall2)
                        //            {
                        //                node2 = xn.InnerText;

                        //            }
                        //        }
                        //        los = Convert.ToString(Convert.ToInt32(node1) + Convert.ToInt32(node2));
                        //    }
                        //    else
                        //        los = Convert.ToString(Convert.ToInt32(node));



                        //    XmlNodeList xnListBal = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/INDV-RESPONSES/PRIMARY-SUMMARY/TOTAL-OTHER-INSTALLMENT-AMOUNT");

                        //    if (xnListBal != null && xnListBal.Count > 0)
                        //    {
                        //        foreach (XmlNode xn in xnListBal)
                        //        {
                        //            nodeemi = xn.InnerText;

                        //        }
                        //    }
                        //    if (!string.IsNullOrEmpty(node))
                        //        existingemi = nodeemi;
                        //    else
                        //        existingemi = "0";
                        //    if (existingemi == "0" && los != "0")
                        //    {
                        //        XmlNodeList xnListBal1 = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/RESPONSES/RESPONSE/LOAN-DETAILS/INSTALLMENT-AMT");
                        //        if (xnListBal1 != null && xnListBal1.Count > 0)
                        //        {
                        //            foreach (XmlNode xn in xnListBal1)
                        //            {
                        //                nodeemi = xn.InnerText.ToString().Split('/')[0].Trim().Replace(",", "");

                        //            }
                        //        }
                        //        if (!string.IsNullOrEmpty(node))
                        //            existingemi = nodeemi;
                        //        else
                        //            existingemi = "0";
                        //    }
                        //}
                        //else
                        //{
                        var myDeserializedClass = JsonConvert.DeserializeObject<CrifJsonResponse>(jsonCrif);
                        if (myDeserializedClass != null)
                        {
                            if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.SCORES.Count > 0)
                            {
                                if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.SCORES[0].SCOREVALUE == null || myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.SCORES[0].SCOREVALUE == "")
                                {
                                    ScoreValue = "0";
                                }
                                else
                                {
                                    ScoreValue = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.SCORES[0].SCOREVALUE;

                                }
                            }
                            var data = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.RESPONSES;
                            var paymentdata = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.RESPONSES;

                            if (data != null)
                            {
                                foreach (var item in data)
                                {
                                    if (item.LOANDETAILS.OVERDUEAMT == null || item.LOANDETAILS.OVERDUEAMT == "")
                                    {
                                        OverdueAmt = "0";
                                    }

                                    else
                                    {
                                        OverdueAmt = item.LOANDETAILS.OVERDUEAMT;
                                        string overduecomma = OverdueAmt.ToString().Replace(",", "");
                                        Overduetotal += int.Parse(overduecomma);
                                    }

                                    if (item.LOANDETAILS.WRITEOFFAMT == null || item.LOANDETAILS.WRITEOFFAMT == "")
                                    {
                                        Writeoff = "0";
                                    }
                                    else
                                    {
                                        Writeoff = item.LOANDETAILS.WRITEOFFAMT;
                                        string writecomma = Writeoff.ToString().Replace(",", "");
                                        Writetotal += int.Parse(writecomma);
                                    }

                                }

                            }
                            if (paymentdata != null)
                            {
                                // Combined Payment History Start
                                foreach (var item in paymentdata)
                                {
                                    PaymentHistory = item.LOANDETAILS.COMBINEDPAYMENTHISTORY;

                                    if (item.LOANDETAILS.COMBINEDPAYMENTHISTORY == null || item.LOANDETAILS.COMBINEDPAYMENTHISTORY == "")
                                    {
                                        PaymentHistory = "0";
                                    }
                                    else
                                    {
                                        var payhistory = PaymentHistory.Trim().Split('|');

                                        if (payhistory.Length > 0)
                                        {

                                            for (int j = 0; j < payhistory.Length; j++)
                                            {
                                                bool isDateCheck = CheckEmiDate(payhistory[j]);
                                                if (isDateCheck)
                                                {
                                                    finaldpd = finaldpd + Convert.ToInt32(!string.IsNullOrEmpty(payhistory[j].ToString()) ? Regex.IsMatch(payhistory[j].ToString().Substring(payhistory[j].ToString().IndexOf(",") + 1, 3), @"^\d+$") ? payhistory[j].ToString().Substring(payhistory[j].ToString().IndexOf(",") + 1, 3) : 0 : 0);
                                                }
                                            }
                                        }
                                    }


                                }
                            }


                            // Installemnt start
                            string indvexistingemi = "0";
                            string grpexistingemi = "0";
                            string installment = string.Empty;
                            string AccountStatus = string.Empty;
                            int finaldp = 0;


                            var listinstall = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.PRIMARYSUMMARY.TOTALOTHERCURRENTBALANCE;

                            if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.PRIMARYSUMMARY.TOTALOTHERCURRENTBALANCE == null)
                            {

                                listinstall = 0;
                            }
                            else
                            {

                                node = Convert.ToString(listinstall);


                                if (node == "0")
                                {
                                    var listinstall12 = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.ACCOUNTSSUMMARY.PRIMARYACCOUNTSSUMMARY.PRIMARYCURRENTBALANCE;
                                    if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.ACCOUNTSSUMMARY.PRIMARYACCOUNTSSUMMARY.PRIMARYCURRENTBALANCE == null)
                                    {
                                        listinstall12 = "0";
                                    }
                                    else
                                    {
                                        if (listinstall12 != null && listinstall12.Length > 0)
                                        {
                                            node1 = Convert.ToString(listinstall12);
                                        }
                                    }

                                    var listinstall14 = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.ACCOUNTSSUMMARY.SECONDARYACCOUNTSSUMMARY.SECONDARYCURRENTBALANCE;
                                    if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.ACCOUNTSSUMMARY.SECONDARYACCOUNTSSUMMARY.SECONDARYCURRENTBALANCE == null)
                                    {

                                    }
                                    else
                                    {
                                        if (listinstall14 != null && listinstall14.Length > 0)
                                        {
                                            node2 = Convert.ToString(listinstall14);
                                        }

                                        los = Convert.ToString(Convert.ToInt32(node1) + Convert.ToInt32(node2));
                                    }
                                }
                                else
                                {
                                    los = Convert.ToString(Convert.ToInt32(node));
                                }

                            }

                            //Balance emi INDV Response
                            string indvnodeemi = "0";
                            var indvbalanceemi = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.PRIMARYSUMMARY.TOTALOTHERINSTALLMENTAMOUNT;
                            if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.PRIMARYSUMMARY.TOTALOTHERINSTALLMENTAMOUNT == null)
                            {
                                indvbalanceemi = 0;
                            }
                            else
                            {
                                if (indvbalanceemi != null)
                                {
                                    indvnodeemi = Convert.ToString(indvbalanceemi);

                                }

                                if (!string.IsNullOrEmpty(node))
                                    indvexistingemi = indvnodeemi;
                                else
                                    indvexistingemi = "0";
                            }


                            //Balance emi GRP Response
                            var grpbalanceemi = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.GRPRESPONSES.SUMMARY.TOTALOTHERINSTALLMENTAMOUNT;
                            if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.GRPRESPONSES.SUMMARY.TOTALOTHERINSTALLMENTAMOUNT == null)
                            {
                                grpbalanceemi = 0;
                            }
                            else
                            {
                                string grpnodeemi = "0";
                                if (grpbalanceemi != null)
                                {
                                    grpnodeemi = Convert.ToString(grpbalanceemi);

                                }

                                if (!string.IsNullOrEmpty(node))
                                    grpexistingemi = grpnodeemi;
                                else
                                    grpexistingemi = "0";


                                if (indvexistingemi == "0" && los != "0")
                                {
                                    var ListBal1 = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.INDVRESPONSELIST.INDVRESPONSE;
                                    if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.INDVRESPONSELIST.INDVRESPONSE == null)
                                    {
                                        int ListBal1s = Convert.ToInt32(ListBal1);
                                        ListBal1s = 0;
                                    }
                                    else
                                    {
                                        if (ListBal1 != null)
                                        {
                                            foreach (var item in ListBal1)
                                            {
                                                if (!string.IsNullOrEmpty(indvnodeemi))
                                                    indvnodeemi = Convert.ToString(Convert.ToInt32(indvnodeemi) + Convert.ToInt32(item.LOANDETAIL.INSTALLMENTAMT.ToString().Split('/')[0].Trim().Replace(",", "")));
                                                else
                                                    indvnodeemi = item.LOANDETAIL.INSTALLMENTAMT.ToString().Split('/')[0].Trim().Replace(",", "");
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(node))
                                            indvexistingemi = indvnodeemi;
                                        else
                                            indvexistingemi = "0";
                                    }

                                }
                            }


                            var listresp = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.RESPONSES;

                            if (listresp != null && listresp.Count > 0)
                            {
                                foreach (var item in listresp)
                                {
                                    var lst = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.RESPONSES;

                                    foreach (var actsts in lst)
                                    {
                                        AccountStatus = actsts.LOANDETAILS.ACCOUNTSTATUS;
                                    }

                                    var payhistory = PaymentHistory.Trim().Split('|');
                                    for (int j = 0; j < payhistory.Length; j++)
                                    {
                                        if (!string.IsNullOrEmpty(payhistory[j]))
                                        {
                                            if (payhistory[j].Substring(9, 3) != "XXX" && payhistory[j].Substring(9, 3) != "DDD")
                                                finaldp = finaldp + Convert.ToInt32(payhistory[j].Substring(9, 3));
                                        }
                                    }
                                }
                            }

                            if (int.TryParse(ScoreValue, out _))
                            {
                                obj.CrifScore = ScoreValue != null && ScoreValue != string.Empty ? Convert.ToInt32(ScoreValue) : 0;

                            }
                            else
                            {
                                obj.CrifScore = 0;

                            }

                            obj.CombinePayHistory = Convert.ToInt32(finaldp);
                            obj.INDVInstallment = !string.IsNullOrEmpty(indvexistingemi) ? Convert.ToInt32(indvexistingemi) : 0;
                            obj.GSPInstallment = !string.IsNullOrEmpty(grpexistingemi) ? Convert.ToInt32(grpexistingemi) : 0;
                            obj.TotalCurrentAmt = !string.IsNullOrEmpty(los) ? Convert.ToInt32(los) : 0;


                        }
                        //}




                        ti = Convert.ToInt32(dataGrid.Rows[irow]["pincome"]) + Convert.ToInt32(dataGrid.Rows[irow]["famincome"]);

                        int rent = Convert.ToInt32(dataGrid.Rows[irow]["Rent"]);
                        int food = Convert.ToInt32(dataGrid.Rows[irow]["Fooding"]);
                        int ent = Convert.ToInt32(dataGrid.Rows[irow]["Entertainment"]);
                        int edu = Convert.ToInt32(dataGrid.Rows[irow]["Education"]);
                        int health = Convert.ToInt32(dataGrid.Rows[irow]["Health"]);
                        int oth = Convert.ToInt32(dataGrid.Rows[irow]["Others"]);
                        totexp = rent + food + ent + edu + health + oth;
                        int pendingincome = ti - totexp;
                        int loanapplied = Convert.ToInt32(dataGrid.Rows[irow]["Loan_amount"].ToString());
                        int loandur = Convert.ToInt32(dataGrid.Rows[irow]["Loan_Duration"].ToString());
                        string bankname = string.Empty;
                        bankname = Convert.ToString(dataGrid.Rows[irow]["P_Phone"]);

                        int exaxtExp = Convert.ToInt32(totexp / loanapplied) * 100;
                        if (exaxtExp < 25)
                            totexp = Convert.ToInt32(loanapplied * 0.25);



                        // ** Base Emi ******
                        if (!string.IsNullOrEmpty(bankname) && (bankname.Trim().ToUpper() == "UCO" || bankname.Trim().ToUpper() == "SBI" || bankname.Trim().ToUpper() == "PNB" || bankname.Trim().ToUpper() == "BOB" || bankname.Trim().ToUpper() == "KTB"))
                            emibase = helper.getemi(bankname, loandur);
                        else
                            emibase = "Bank Not Selected";

                        int lappr = 0;
                        int eighty = 0;
                        int twenty = 0;
                        if (string.IsNullOrEmpty(emibase))
                        {
                            return "<div class='container'><h2>Emi Master is missing. Please contact IT Support.</h2></div>";
                        }
                        if (emibase != "Bank Not Selected")
                        {
                            DataTable Bankrule = helper.getBREdata(bankname, loandur);
                            if (Bankrule.Rows.Count > 0)
                            {
                                for (int i = 0; i < Bankrule.Rows.Count; i++)
                                {
                                    var loanAmt = Bankrule.Rows[i]["LoanSlab"].ToString();
                                    var age1 = Bankrule.Rows[i]["Age"].ToString();
                                    var duration = Bankrule.Rows[i]["Tenure"].ToString();
                                    var femaleeligleble = Bankrule.Rows[i]["FemaleEligibilty"].ToString();
                                    var WorkingMaleEligibilty = Bankrule.Rows[i]["WorkingMaleEligibilty"].ToString();
                                    var MinCreditScore = Bankrule.Rows[i]["MinCreditScore"].ToString();
                                    var MaxCreditScore = Bankrule.Rows[i]["MaxCreditScore"].ToString();
                                    var DPD = Bankrule.Rows[i]["DPD"].ToString();
                                    int GrossExposure = Convert.ToInt32(Bankrule.Rows[i]["GrossExposure"]);

                                    string[] amtArr = loanAmt.Split("-");
                                    int minbankAmount = Convert.ToInt32(amtArr[0]);
                                    int maxbankAmount = Convert.ToInt32(amtArr[1]);

                                    // bool IsApproved = false;
                                    string dpd = DPD.Contains("-") ? DPD : "0-" + DPD;
                                    string[] adDpd = dpd.Split("-");

                                    if (Convert.ToInt32(adDpd[1]) < finaldpd && Convert.ToInt32(adDpd[1]) != 0 && adDpd.Length == 2)
                                    {
                                        camhtml += "   <td colspan='8' style='text-align:center;'>DPD is Greater than " + Convert.ToInt32(adDpd[1]) + " so case is rejected </td>";
                                    }
                                    else
                                    {
                                        string[] ageArr = age1.Split("-");
                                        int minbankAge = Convert.ToInt32(ageArr[0]);
                                        int maxbankAge = Convert.ToInt32(ageArr[1]);
                                        string DobYear1 = Convert.ToDateTime(dataGrid.Rows[irow]["DOB"]).Year.ToString();
                                        int dobyear = Convert.ToInt32(DobYear1);

                                        int toyear = Convert.ToInt32(DateTime.Now.Year);
                                        int custAge = toyear - dobyear;

                                        if (minbankAge <= custAge && maxbankAge >= custAge)
                                        {

                                            string[] tenureArr = duration.Split("-");
                                            bool IsApproved = tenureArr.Contains(Convert.ToString(loandur));
                                            if (!IsApproved)
                                            {
                                                camhtml += "   <td colspan='8' style='text-align:center;'>Duration Creteria does not match in our system  so case is rejected </td>";
                                            }
                                            else
                                            {

                                                //**** total emi on loan applied ******
                                                double EmiBase = Convert.ToDouble(emibase);
                                                int calcEmi = (int)Math.Round(((EmiBase / 100000) * loanapplied), 0);
                                                if (Convert.ToInt64(existingemi) < pendingincome)
                                                {
                                                    if (pendingincome > Convert.ToInt32(calcEmi + Convert.ToInt32(1000)))
                                                    {

                                                        lappr = Convert.ToInt32(dataGrid.Rows[irow]["Loan_Amt"]);
                                                        if (creator.ToString().Substring(0, 2).ToUpper() == "VH")
                                                        {
                                                            eighty = Convert.ToInt32((lappr * 80) / 100);
                                                            twenty = Convert.ToInt32((lappr * 20) / 100);
                                                        }
                                                        else if ((Convert.ToInt32(los) + lappr) < Convert.ToInt32(150000))
                                                        {
                                                            eighty = Convert.ToInt32((lappr * 80) / 100);
                                                            twenty = Convert.ToInt32((lappr * 20) / 100);
                                                        }
                                                        else
                                                        {
                                                            int allowedamount = Convert.ToInt32(150000) - Convert.ToInt32(los) + lappr;
                                                            lappr = allowedamount - 1000;
                                                            eighty = Convert.ToInt32((lappr * 80) / 100);
                                                            twenty = Convert.ToInt32((lappr * 20) / 100);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lappr = (int)Math.Round((100000 / EmiBase) * Convert.ToInt32(pendingincome - 1000), 0);
                                                        if ((Convert.ToInt32(los) + lappr) < Convert.ToInt32(150000))
                                                        {
                                                            eighty = Convert.ToInt32((lappr * 80) / 100);
                                                            twenty = Convert.ToInt32((lappr * 20) / 100);
                                                        }
                                                        else
                                                        {
                                                            int allowedamount = Convert.ToInt32(150000) - Convert.ToInt32(los) + lappr;
                                                            lappr = allowedamount - 1000;
                                                            eighty = Convert.ToInt32((lappr * 80) / 100);
                                                            twenty = Convert.ToInt32((lappr * 20) / 100);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    lappr = 0;
                                                    dataGrid.Rows[irow]["LoanApproved"] = "0";
                                                }
                                                if (lappr > 0)
                                                    dataGrid.Rows[irow]["LoanApproved"] = lappr.ToString();
                                                else
                                                    dataGrid.Rows[irow]["LoanApproved"] = "0";
                                                dataGrid.Rows[irow]["80"] = eighty.ToString();
                                                dataGrid.Rows[irow]["20"] = twenty.ToString();
                                                // camhtml += "   <td style='text-align: center;'>" + lappr + "  </td>";
                                                int crscore = 0;
                                                if (!string.IsNullOrEmpty(Convert.ToString(dataGrid.Rows[irow]["CrifScore"])))
                                                {
                                                    crscore = Convert.ToInt32(Convert.ToString(dataGrid.Rows[irow]["CrifScore"]));
                                                    if (crscore > Convert.ToInt32(MinCreditScore) && crscore < Convert.ToInt32(MaxCreditScore))
                                                    {
                                                        camhtml += "   <td colspan='8' style='text-align:center;'>Crif Score is less than " + MaxCreditScore + " so case is rejected </td>";
                                                        dataGrid.Rows[irow]["LoanApproved"] = "0";
                                                    }
                                                    else if (lappr < 0)
                                                        camhtml += "   <td colspan='8' style='text-align:center;'>Total Outstanding is very High so case is rejected </td>";
                                                    else
                                                        camhtml += "<td colspan='8' style='text-align:center;'></td>";
                                                }
                                                else
                                                    camhtml += "<td colspan='8' style='text-align:center;'></td>";


                                            }
                                        }
                                        else
                                        {
                                            camhtml += "   <td colspan='8' style='text-align:center;'>Age Creteria does not match in our system so case is rejected </td>";
                                        }



                                    }
                                    //   camhtml += "</tr>";
                                }
                            }
                        }

                        else
                        {
                            dataGrid.Rows[irow]["LoanApproved"] = "0";
                            dataGrid.Rows[irow]["80"] = "0";
                            dataGrid.Rows[irow]["20"] = "0";
                            camhtml += "   <td style='text-align: center;'>Bank Tag missing in Fi</td>";
                            camhtml += "   <td colspan='8' style='text-align:center;'></td>";
                            //  camhtml += "</tr>";
                        }
                        camhtml += "</tr>";
                    }

                    camhtml += " <tr style=' background: #e5dfdf;  padding: 5px 0px; margin-top: 10px;' >";
                    camhtml += "  <td colspan='5' style='padding-left: 10px;' ><b>TOTAL LOAN Amount</b></td>";
                    camhtml += "  <td></td>";
                    camhtml += "  <td></td>";
                    camhtml += "  <td></td>";
                    camhtml += "  <td style='text-align:center;'></td>";
                    camhtml += "  <td></td>";
                    camhtml += "  <td colspan='2' style='text-align:center;'></td>";

                    camhtml += "   <td colspan='2' ></td>";
                    camhtml += "</tr>";
                    camhtml += "</table>";
                    camhtml += "</div>";
                    camhtml += "<div class='table-responsive'>";
                    camhtml += " <table style='width:100%'>";
                    camhtml += "<tr >";
                    camhtml += " <td style='padding-left: 10px;' >Borrower S.NO.</td>";
                    int rowc = 0;
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        rowc++;
                        camhtml += " <td style='text-align: center;'>" + rowc + "</td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += " <td style='padding-left: 10px;' >Purpose of loan</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        camhtml += " <td style='text-align: center;'><b>" + Convert.ToString(dataGrid.Rows[irows]["Loan_Reason"]) + "</b></td>";
                    }

                    camhtml += "</tr>";
                    camhtml += " <tr>";
                    camhtml += "  <td style='padding-left: 10px;' >Crif Score</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        if (Convert.ToString(dataGrid.Rows[irows]["CrifScore"]) != "")
                            camhtml += "  <td style='text-align: center;'>" + Convert.ToString(dataGrid.Rows[irows]["CrifScore"]) + "</td>";
                        else
                        {
                            string getcrifxml = helper.crifdata(creator.ToString().ToUpper().Trim(), dataGrid.Rows[irows]["Code"].ToString().ToUpper());
                            if (!string.IsNullOrEmpty(getcrifxml))
                            {
                                string xmlrep = Encoding.UTF8.GetString(Convert.FromBase64String(getcrifxml));
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(xmlrep);
                                XmlNodeList xnList = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/SCORES/SCORE/SCORE-VALUE");
                                string node = "";
                                if (xnList != null && xnList.Count > 0)
                                {
                                    foreach (XmlNode xn in xnList)
                                    {
                                        node = xn.InnerText;

                                    }
                                }
                                camhtml += "  <td style='text-align: center;'>" + node + "</td>";
                            }
                            else
                            {
                                camhtml += "  <td style='text-align: center;'>" + ScoreValue + "</td>";
                            }

                        }
                    }
                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += " <td style='padding-left: 10px;' >Status FTB/Exp</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        if (Convert.ToString(dataGrid.Rows[irows]["CrifScore"]) != "" && Convert.ToString(dataGrid.Rows[irows]["CrifScore"]) != null)
                        {
                            if (Convert.ToInt32(Convert.ToString(dataGrid.Rows[irows]["CrifScore"])) > 300)
                                camhtml += "  <td style='text-align: center;'>EXP</td>";
                            else
                                camhtml += "  <td style='text-align: center;'>FTB</td>";
                        }
                        else
                            camhtml += "  <td style='text-align: center;'>FTB</td>";
                    }

                    camhtml += "</tr>";
                    camhtml += " <tr>";

                    camhtml += " <td style='padding-left: 10px;' width: >Residential Stability (IN MONTHS)ss</td>";

                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        int home = 0;
                        if (!string.IsNullOrEmpty(Convert.ToString(dataGrid.Rows[irows]["Live_In_Present_Place"])))
                        {
                            home = Convert.ToInt32(Convert.ToInt32(Convert.ToString(dataGrid.Rows[irows]["Live_In_Present_Place"])) * 12);
                        }

                        camhtml += "<td style='text-align: center;'> " + home + " </td>";

                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += " <td style='padding-left: 10px;' >Banking (in months) </td>";

                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {

                        camhtml += " <td style='text-align: center;'> N/A </td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td colspan='16' style='padding-left: 10px;' ><b>Calculation of Maximum Permissible Finance </b></td>";

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px;' >Monthly business Income (A)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {

                        camhtml += "<td></td>";
                    }


                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px; width: 140px;' >Monthly Business Expense (B)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        camhtml += "<td></td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px; width: 140px;' >Profit/Loss (A-B=C)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        camhtml += "   <td style='padding-left: 10px;' >" + dataGrid.Rows[irows]["pincome"].ToString() + "</td>";


                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px; width: 140px;' >Any other source of income (D)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        camhtml += "   <td style='padding-left: 10px;' >" + dataGrid.Rows[irows]["famincome"].ToString() + "</td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px; width: 140px;' >Total Income  (C+D=E)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        ti = Convert.ToInt32(dataGrid.Rows[irows]["pincome"]) + Convert.ToInt32(dataGrid.Rows[irows]["famincome"]);
                        //Session["ti"] += ti + "-";
                        tii += ti + "-";

                        camhtml += "   <td style='padding-left: 10px;' >" + ti + "</td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px; width: 140px;' >Total Existing EMIs CRIF(F)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {

                        string getcrifxml = helper.crifdata(creator.ToString().ToUpper().Trim(), dataGrid.Rows[irows]["Code"].ToString().ToUpper());
                        if (!string.IsNullOrEmpty(getcrifxml))
                        {
                            string xmlrep = Encoding.UTF8.GetString(Convert.FromBase64String(getcrifxml));
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(xmlrep);
                            XmlNodeList xnListBal = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/INDV-RESPONSES/PRIMARY-SUMMARY/TOTAL-OTHER-INSTALLMENT-AMOUNT");
                            string node = "";
                            if (xnListBal != null && xnListBal.Count > 0)
                            {
                                foreach (XmlNode xn in xnListBal)
                                {
                                    node = xn.InnerText;

                                }
                            }
                            if (!string.IsNullOrEmpty(node))
                                existingemi = node;
                            else
                                existingemi = "0";
                            if (existingemi == "0")
                            {
                                XmlNodeList xnListBal1 = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/RESPONSES/RESPONSE/LOAN-DETAILS/INSTALLMENT-AMT");
                                if (xnListBal1 != null && xnListBal1.Count > 0)
                                {
                                    foreach (XmlNode xn in xnListBal1)
                                    {
                                        node = xn.InnerText.ToString().Split('/')[0].Trim().Replace(",", "");

                                    }
                                }
                                if (!string.IsNullOrEmpty(node))
                                    existingemi = node;
                                else
                                    existingemi = "0";
                            }
                            //Session["extemi"] += existingemi + "-";
                            existingemii += existingemi + "-";
                            camhtml += "   <td>" + existingemi + "</td>";
                        }
                        else
                        {
                            existingemii = Convert.ToString(obj.INDVInstallment);
                            camhtml += "<td>" + obj.INDVInstallment + "</td>";
                        }
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px; width: 140px;' >Household Expenses(G)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        int rent = Convert.ToInt32(dataGrid.Rows[irows]["Rent"]);
                        int food = Convert.ToInt32(dataGrid.Rows[irows]["Fooding"]);
                        int ent = Convert.ToInt32(dataGrid.Rows[irows]["Entertainment"]);
                        int edu = Convert.ToInt32(dataGrid.Rows[irows]["Education"]);
                        int health = Convert.ToInt32(dataGrid.Rows[irows]["Health"]);
                        int oth = Convert.ToInt32(dataGrid.Rows[irows]["Others"]);
                        totexp = rent + food + ent + edu + health + oth;
                        //Session["totexp"] += totexp + "-";
                        totexpp += totexp + "-";
                        camhtml += "   <td>" + totexp + "</td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px; width: 140px;' >Eligible EMI (E-F-G = H)</td>";

                    var tinc = tii.Split('-'); // Session["ti"].ToString().Split('-');
                    var exemi = existingemii.Split('-');// Session["extemi"].ToString().Split('-');
                    var texp = totexpp.Split('-');// Session["totexp"].ToString().Split('-');
                    int elgemi = 0;
                    //for (var exp = 0; exp < tinc.Length - 1; exp++)
                    //{
                    //    elgemi = Convert.ToInt32(tinc[exp]) - exemi.Length>1? Convert.ToInt32(exemi[exp]): Convert.ToInt32(exemi[0]);
                    //    elgemi = elgemi - Convert.ToInt32(texp[exp]);
                    //    camhtml += "   <td>" + elgemi + "</td>";
                    //}

                    int minLength = Math.Min(Math.Min(tinc.Length, exemi.Length - 1), texp.Length);
                    int[] elgemiArray = new int[minLength];
                    for (var exp = 0; exp < minLength; exp++)
                    {
                        int tincValue = Convert.ToInt32(tinc[exp]);
                        int exemiValue = Convert.ToInt32(exemi[exp]);
                        int texpValue = Convert.ToInt32(texp[exp]);
                        int elgemis = tincValue - exemiValue;
                        elgemi = elgemis - texpValue;
                        elgemiArray[exp] = elgemi;
                        camhtml += "   <td>" + elgemi + "</td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px;width: 140px;' >Requested Loan EMI (I)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        camhtml += "   <td></td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += " <td style='padding-left: 10px;' >Pro Loan Amt (X) (Lwr of H&I)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {

                        camhtml += " <td></td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px;' >Proposed EMI</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        int proposeemi = 0;

                        if (emibase != "Bank Not Selected")
                        {
                            double EmiBase = Convert.ToDouble(emibase);
                            if (!string.IsNullOrEmpty(dataGrid.Rows[irows]["LoanApproved"].ToString()))
                                proposeemi = (int)Math.Round(((EmiBase / 100000) * Convert.ToInt32(dataGrid.Rows[irows]["LoanApproved"].ToString())), 0);
                        }
                        //proposeemi = Convert.ToInt32(Convert.ToInt32(Convert.ToInt32(emibase) / 1000000) * Convert.ToInt32(dataGrid.Rows[irows]["LoanApproved"].ToString()));
                        else
                            proposeemi = 0;
                        camhtml += "<td>" + proposeemi + "</td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px;' >Bank Share (80% of x)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {

                        camhtml += "   <td> " + dataGrid.Rows[irows]["80"].ToString() + "</td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr> ";
                    camhtml += "   <td style='padding-left: 10px;' >PDL share (20% of X)</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        camhtml += "   <td> " + dataGrid.Rows[irows]["20"].ToString() + "</td>";
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "  <td style='padding-left: 10px;' >LOAN O/S OTHERS</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        string los = string.Empty;
                        string OverdueAmt = "";
                        int Overduetotal = 0;
                        string Writeoff = "";
                        int Writetotal = 0;
                        string PaymentHistory = "";
                        int finaldpd = 0;
                        string getcrifxml = helper.crifdata(creator.ToString().ToUpper().Trim(), dataGrid.Rows[irows]["Code"].ToString().ToUpper());
                        string crifjson = helper.GetJsonCrif(creator.ToString().ToUpper().Trim(), dataGrid.Rows[irows]["Code"].ToString().ToUpper(), _credManager, dbName, isLive);
                        if (!string.IsNullOrEmpty(getcrifxml))
                        {
                            string xmlrep = Encoding.UTF8.GetString(Convert.FromBase64String(getcrifxml));
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(xmlrep);
                            XmlNodeList xnListinstall = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/INDV-RESPONSES/PRIMARY-SUMMARY/TOTAL-OTHER-CURRENT-BALANCE");
                            string node = "0";
                            if (xnListinstall != null && xnListinstall.Count > 0)
                            {
                                foreach (XmlNode xn in xnListinstall)
                                {
                                    node = xn.InnerText;

                                }
                            }
                            string node1 = "0";
                            string node2 = "0";
                            if (node == "0")
                            {
                                XmlNodeList xnListinstall1 = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/ACCOUNTS-SUMMARY/PRIMARY-ACCOUNTS-SUMMARY/PRIMARY-CURRENT-BALANCE");
                                if (xnListinstall1 != null && xnListinstall1.Count > 0)
                                {
                                    foreach (XmlNode xn in xnListinstall1)
                                    {
                                        node1 = xn.InnerText;

                                    }
                                }

                                XmlNodeList xnListinstall2 = xmlDoc.SelectNodes("/INDV-REPORT-FILE/INDV-REPORTS/INDV-REPORT/ACCOUNTS-SUMMARY/SECONDARY-ACCOUNTS-SUMMARY/SECONDARY-CURRENT-BALANCE");
                                if (xnListinstall2 != null && xnListinstall2.Count > 0)
                                {
                                    foreach (XmlNode xn in xnListinstall2)
                                    {
                                        node2 = xn.InnerText;

                                    }
                                }
                                los = Convert.ToString(Convert.ToInt32(node1) + Convert.ToInt32(node2));
                            }
                            else
                                los = Convert.ToString(Convert.ToInt32(node));



                            camhtml += "   <td>" + los + "</td>";
                        }
                        else
                        {
                            /*JSON CRIF*/

                            var myDeserializedClass = JsonConvert.DeserializeObject<CrifJsonResponse>(crifjson);
                            if (myDeserializedClass != null)
                            {
                                if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.SCORES.Count > 0)
                                {
                                    if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.SCORES[0].SCOREVALUE == null || myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.SCORES[0].SCOREVALUE == "")
                                    {
                                        ScoreValue = "0";
                                    }
                                    else
                                    {
                                        ScoreValue = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.SCORES[0].SCOREVALUE;

                                    }
                                }
                                var data = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.RESPONSES;
                                var paymentdata = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.RESPONSES;

                                if (data != null)
                                {
                                    foreach (var item in data)
                                    {
                                        if (item.LOANDETAILS.OVERDUEAMT == null || item.LOANDETAILS.OVERDUEAMT == "")
                                        {
                                            OverdueAmt = "0";
                                        }

                                        else
                                        {
                                            OverdueAmt = item.LOANDETAILS.OVERDUEAMT;
                                            string overduecomma = OverdueAmt.ToString().Replace(",", "");
                                            Overduetotal += int.Parse(overduecomma);
                                        }

                                        if (item.LOANDETAILS.WRITEOFFAMT == null || item.LOANDETAILS.WRITEOFFAMT == "")
                                        {
                                            Writeoff = "0";
                                        }
                                        else
                                        {
                                            Writeoff = item.LOANDETAILS.WRITEOFFAMT;
                                            string writecomma = Writeoff.ToString().Replace(",", "");
                                            Writetotal += int.Parse(writecomma);
                                        }

                                    }

                                }
                                if (paymentdata != null)
                                {
                                    // Combined Payment History Start
                                    foreach (var item in paymentdata)
                                    {
                                        PaymentHistory = item.LOANDETAILS.COMBINEDPAYMENTHISTORY;

                                        if (item.LOANDETAILS.COMBINEDPAYMENTHISTORY == null || item.LOANDETAILS.COMBINEDPAYMENTHISTORY == "")
                                        {
                                            PaymentHistory = "0";
                                        }
                                        else
                                        {
                                            var payhistory = PaymentHistory.Trim().Split('|');

                                            if (payhistory.Length > 0)
                                            {

                                                for (int j = 0; j < payhistory.Length; j++)
                                                {
                                                    bool isDateCheck = CheckEmiDate(payhistory[j]);
                                                    if (isDateCheck)
                                                    {
                                                        finaldpd = finaldpd + Convert.ToInt32(!string.IsNullOrEmpty(payhistory[j].ToString()) ? Regex.IsMatch(payhistory[j].ToString().Substring(payhistory[j].ToString().IndexOf(",") + 1, 3), @"^\d+$") ? payhistory[j].ToString().Substring(payhistory[j].ToString().IndexOf(",") + 1, 3) : 0 : 0);
                                                    }
                                                }
                                            }
                                        }


                                    }
                                }


                                // Installemnt start
                                string indvexistingemi = "0";
                                string grpexistingemi = "0";
                                string installment = string.Empty;
                                string AccountStatus = string.Empty;
                                int finaldp = 0;
                                string node = "";
                                string node1 = "";
                                string node2 = "";

                                var listinstall = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.PRIMARYSUMMARY.TOTALOTHERCURRENTBALANCE;

                                if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.PRIMARYSUMMARY.TOTALOTHERCURRENTBALANCE == null)
                                {

                                    listinstall = 0;
                                }
                                else
                                {

                                    node = Convert.ToString(listinstall);


                                    if (node == "0")
                                    {
                                        var listinstall12 = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.ACCOUNTSSUMMARY.PRIMARYACCOUNTSSUMMARY.PRIMARYCURRENTBALANCE;
                                        if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.ACCOUNTSSUMMARY.PRIMARYACCOUNTSSUMMARY.PRIMARYCURRENTBALANCE == null)
                                        {
                                            listinstall12 = "0";
                                        }
                                        else
                                        {
                                            if (listinstall12 != null && listinstall12.Length > 0)
                                            {
                                                node1 = Convert.ToString(listinstall12);
                                            }
                                        }

                                        var listinstall14 = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.ACCOUNTSSUMMARY.SECONDARYACCOUNTSSUMMARY.SECONDARYCURRENTBALANCE;
                                        if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.ACCOUNTSSUMMARY.SECONDARYACCOUNTSSUMMARY.SECONDARYCURRENTBALANCE == null)
                                        {

                                        }
                                        else
                                        {
                                            if (listinstall14 != null && listinstall14.Length > 0)
                                            {
                                                node2 = Convert.ToString(listinstall14);
                                            }

                                            los = Convert.ToString(Convert.ToInt32(node1) + Convert.ToInt32(node2));
                                        }
                                    }
                                    else
                                    {
                                        los = Convert.ToString(Convert.ToInt32(node));
                                    }

                                }

                                //Balance emi INDV Response
                                string indvnodeemi = "0";
                                var indvbalanceemi = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.PRIMARYSUMMARY.TOTALOTHERINSTALLMENTAMOUNT;
                                if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.PRIMARYSUMMARY.TOTALOTHERINSTALLMENTAMOUNT == null)
                                {
                                    indvbalanceemi = 0;
                                }
                                else
                                {
                                    if (indvbalanceemi != null)
                                    {
                                        indvnodeemi = Convert.ToString(indvbalanceemi);

                                    }

                                    if (!string.IsNullOrEmpty(node))
                                        indvexistingemi = indvnodeemi;
                                    else
                                        indvexistingemi = "0";
                                }


                                //Balance emi GRP Response
                                var grpbalanceemi = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.GRPRESPONSES.SUMMARY.TOTALOTHERINSTALLMENTAMOUNT;
                                if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.GRPRESPONSES.SUMMARY.TOTALOTHERINSTALLMENTAMOUNT == null)
                                {
                                    grpbalanceemi = 0;
                                }
                                else
                                {
                                    string grpnodeemi = "0";
                                    if (grpbalanceemi != null)
                                    {
                                        grpnodeemi = Convert.ToString(grpbalanceemi);

                                    }

                                    if (!string.IsNullOrEmpty(node))
                                        grpexistingemi = grpnodeemi;
                                    else
                                        grpexistingemi = "0";


                                    if (indvexistingemi == "0" && los != "0")
                                    {
                                        var ListBal1 = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.INDVRESPONSELIST.INDVRESPONSE;
                                        if (myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.INDVRESPONSES.INDVRESPONSELIST.INDVRESPONSE == null)
                                        {
                                            int ListBal1s = Convert.ToInt32(ListBal1);
                                            ListBal1s = 0;
                                        }
                                        else
                                        {
                                            if (ListBal1 != null)
                                            {
                                                foreach (var item in ListBal1)
                                                {
                                                    if (!string.IsNullOrEmpty(indvnodeemi))
                                                        indvnodeemi = Convert.ToString(Convert.ToInt32(indvnodeemi) + Convert.ToInt32(item.LOANDETAIL.INSTALLMENTAMT.ToString().Split('/')[0].Trim().Replace(",", "")));
                                                    else
                                                        indvnodeemi = item.LOANDETAIL.INSTALLMENTAMT.ToString().Split('/')[0].Trim().Replace(",", "");
                                                }
                                            }

                                            if (!string.IsNullOrEmpty(node))
                                                indvexistingemi = indvnodeemi;
                                            else
                                                indvexistingemi = "0";
                                        }

                                    }
                                }


                                var listresp = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.RESPONSES;

                                if (listresp != null && listresp.Count > 0)
                                {
                                    foreach (var item in listresp)
                                    {
                                        var lst = myDeserializedClass.INDVREPORTFILE.INDVREPORTS[0].INDVREPOR.RESPONSES;

                                        foreach (var actsts in lst)
                                        {
                                            AccountStatus = actsts.LOANDETAILS.ACCOUNTSTATUS;
                                        }

                                        var payhistory = PaymentHistory.Trim().Split('|');
                                        for (int j = 0; j < payhistory.Length; j++)
                                        {
                                            if (!string.IsNullOrEmpty(payhistory[j]))
                                            {
                                                if (payhistory[j].Substring(9, 3) != "XXX" && payhistory[j].Substring(9, 3) != "DDD")
                                                    finaldp = finaldp + Convert.ToInt32(payhistory[j].Substring(9, 3));
                                            }
                                        }
                                    }
                                }
                                if (int.TryParse(ScoreValue, out _))
                                {
                                    obj.CrifScore = !string.IsNullOrEmpty(ScoreValue) ? Convert.ToInt32(ScoreValue) : 0;
                                }
                                else
                                {
                                    obj.CrifScore =  0;
                                }

                                
                                obj.CombinePayHistory = Convert.ToInt32(finaldp);
                                obj.INDVInstallment = !string.IsNullOrEmpty(indvexistingemi) ? Convert.ToInt32(indvexistingemi) : 0;
                                obj.GSPInstallment = !string.IsNullOrEmpty(grpexistingemi) ? Convert.ToInt32(grpexistingemi) : 0;
                                obj.TotalCurrentAmt = !string.IsNullOrEmpty(los) ? Convert.ToInt32(los) : 0;

                                camhtml += "   <td>" + obj.TotalCurrentAmt + "</td>";
                            }
                        }
                    }

                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td style='padding-left: 10px;' >LOAN O/S OUR'S</td>";
                    for (var irows = 0; irows < dataGrid.Rows.Count; irows++)
                    {
                        camhtml += "   <td></td>";
                    }

                    camhtml += "</tr>";

                    camhtml += "<tr>";
                    camhtml += "  <td colspan='16' style='background:ghostwhite;padding:8px'></td>";
                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "  <td colspan='6' style='padding-left: 10px;' >Date " + System.DateTime.Now.ToString("dd-MM-yyyy") + " <br><br><br><br>Signature and Name Loan Processing</td>";
                    camhtml += "  <td colspan='10' style='padding-left: 10px;' > Date <br><br><br><br>Name and Signature Approving Authority PDL</td>";
                    camhtml += "</tr>";
                    camhtml += "<tr>";
                    camhtml += "   <td colspan='6' style='padding-left: 10px;' >Date <br><br><br><br>Signature and Name Audit Officer</td>";
                    camhtml += "   <td colspan='10' style='padding-left: 10px;' > Date<br><br><br><br>Name, Signature Approving Authority BOB</td>";
                    camhtml += " </tr>";
                    camhtml += "</table>";
                    camhtml += "</div>";


                    camhtml += "</div>";
                    return camhtml;
                }

            }
            return null;
        }


        public static bool CheckEmiDate(string Datevalue)
        {
            bool isWithinLastSixMonths = false; // Initialize to false (assuming date is not within range)
            try
            {
                // Extract month and year from the input string
                string monthString = Datevalue.Substring(0, 3); // Extracts "Sep"
                string yearString = Datevalue.Substring(4, 4); // Extracts "2016"
                                                               // Parse the month and year to a DateTime object
                DateTime date = DateTime.ParseExact(monthString + " " + yearString, "MMM yyyy", CultureInfo.InvariantCulture);
                //DateTime date = DateTime.Now;
                // Get the current date and the date six months ago
                DateTime currentDate = DateTime.Now;
                DateTime sixMonthsAgo = currentDate.AddMonths(-6);
                // Check if the parsed date is within the last six months
                isWithinLastSixMonths = date >= sixMonthsAgo && date <= currentDate;
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., log it or throw a specific exception
                Console.WriteLine("Error processing date: " + ex.Message);
                // Optionally rethrow the exception if needed
                // throw;
            }
            return isWithinLastSixMonths;
        }

        public List<string> GetFiCodeByCreator(int CreatorId, string dbName, bool isLive)
        {
            List<string> res = new List<string>();
            string query = "Usp_GetCamGeneration";

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                con.Open();
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "GetFiCodeByCreator");
                    cmd.Parameters.AddWithValue("@CreatorID", CreatorId);
                   
                       using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res.Add(reader["FiCode"].ToString()); // assuming FiCode column exists
                            }
                       }
                   
                }
            }
            return res;
        }


        public List<BranchTypeWiseReportVM> GetBranchTypeWiseReport(
        BranchTypeWiseReportVM model,
        string dbName,
        bool isLive,
        out List<FIInfoVM> fiDetails)
        {
            List<BranchTypeWiseReportVM> res = new();
            fiDetails = new();

            using (SqlConnection con = _credManager.getConnections(dbName, isLive))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("USP_GetBranchByCreator_TypeWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SearchMode", model.SearchMode ?? string.Empty);
                    cmd.Parameters.AddWithValue("@CreatorID",
                        model.CreatorId.HasValue ? (object)model.CreatorId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@BranchCode",
                        string.IsNullOrEmpty(model.BranchCode) ? DBNull.Value : model.BranchCode);
                    cmd.Parameters.AddWithValue("@Type", model.Type ?? string.Empty);
                    cmd.Parameters.AddWithValue("@FromDate",
                        model.FromDate.HasValue ? (object)model.FromDate.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate",
                        model.ToDate.HasValue ? (object)model.ToDate.Value : DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        bool HasColumn(string columnName) =>
                            Enumerable.Range(0, reader.FieldCount)
                                      .Any(i => reader.GetName(i)
                                      .Equals(columnName, StringComparison.InvariantCultureIgnoreCase));

                        /* =========================
                           RESULT SET 1 → Branches
                           ========================= */
                        while (reader.Read())
                        {
                            res.Add(new BranchTypeWiseReportVM
                            {
                                SearchMode = model.SearchMode,
                                Type = model.Type,

                                CreatorId = HasColumn("CreatorID") && reader["CreatorID"] != DBNull.Value
                                            ? Convert.ToInt32(reader["CreatorID"])
                                            : (int?)null,

                                CreatorName = HasColumn("CreatorName")
                                              ? reader["CreatorName"]?.ToString()
                                              : null,

                                BranchCode = HasColumn("BranchCode")
                                              ? reader["BranchCode"]?.ToString()
                                              : null,

                                BranchName = HasColumn("BranchName")
                                              ? reader["BranchName"]?.ToString()
                                              : null
                            });
                        }

                        /* =========================
                           RESULT SET 2 → FI DETAILS
                           ========================= */
                        if (reader.NextResult())
                        {
                            bool HasFIColumn(string columnName) =>
                                Enumerable.Range(0, reader.FieldCount)
                                          .Any(i => reader.GetName(i)
                                          .Equals(columnName, StringComparison.InvariantCultureIgnoreCase));

                            while (reader.Read())
                            {
                                fiDetails.Add(new FIInfoVM
                                {
                                    FICode = HasFIColumn("FICode")
                                             ? reader["FICode"]?.ToString()
                                             : null,

                                    // ✅ NEW (from SQL CONCAT)
                                    Name = HasFIColumn("Name")
                                           ? reader["Name"]?.ToString()
                                           : null,

                                    // ✅ DISBURSED only
                                    SmCode = HasFIColumn("SmCode")
                                             ? reader["SmCode"]?.ToString()
                                             : null,

                                    CreatorId = HasFIColumn("CreatorID") && reader["CreatorID"] != DBNull.Value
                                                ? Convert.ToInt32(reader["CreatorID"])
                                                : (int?)null,

                                    CreatorName = HasFIColumn("CreatorName")
                                                  ? reader["CreatorName"]?.ToString()
                                                  : null,

                                    BranchCode = HasFIColumn("Branch_code")
                                                 ? reader["Branch_code"]?.ToString()
                                                 : null,

                                    BranchName = HasFIColumn("BranchName")
                                                 ? reader["BranchName"]?.ToString()
                                                 : null,

                                    // ✅ ESIGN only
                                    EsignStatus = HasFIColumn("BorrSignStatus")
                                                  ? reader["BorrSignStatus"]?.ToString()
                                                  : null
                                });
                            }
                        }
                    }
                }
            }

            return res;
        }







    }
}
