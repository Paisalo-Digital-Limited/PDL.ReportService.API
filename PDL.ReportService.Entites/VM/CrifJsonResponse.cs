using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{

    public class CrifJsonResponse
    {
        [JsonProperty("INDV-REPORT-FILE")]
        public INDVREPORTFILE INDVREPORTFILE { get; set; }
    }
    public class ACCOUNTSSUMMARY
    {
        [JsonProperty("DERIVED-ATTRIBUTES")]
        public DERIVEDATTRIBUTES DERIVEDATTRIBUTES { get; set; }

        [JsonProperty("PRIMARY-ACCOUNTS-SUMMARY")]
        public PRIMARYACCOUNTSSUMMARY PRIMARYACCOUNTSSUMMARY { get; set; }

        [JsonProperty("SECONDARY-ACCOUNTS-SUMMARY")]
        public SECONDARYACCOUNTSSUMMARY SECONDARYACCOUNTSSUMMARY { get; set; }
    }

    public class ADDRESSES
    {
        public List<string> ADDRESS { get; set; }
    }

    public class ADDRESSVARIATION
    {
        public string VALUE { get; set; }

        [JsonProperty("REPORTED-DATE")]
        public string REPORTEDDATE { get; set; }
    }

    public class ALERT
    {
        [JsonProperty("ALERT-DESC")]
        public string ALERTDESC { get; set; }
    }

    public class DATEOFBIRTHVARIATION
    {
        public string VALUE { get; set; }

        [JsonProperty("REPORTED-DATE")]
        public string REPORTEDDATE { get; set; }
    }

    public class DERIVEDATTRIBUTES
    {
        [JsonProperty("INQUIRIES-IN-LAST-SIX-MONTHS")]
        public int INQUIRIESINLASTSIXMONTHS { get; set; }

        [JsonProperty("LENGTH-OF-CREDIT-HISTORY-YEAR")]
        public int LENGTHOFCREDITHISTORYYEAR { get; set; }

        [JsonProperty("LENGTH-OF-CREDIT-HISTORY-MONTH")]
        public int LENGTHOFCREDITHISTORYMONTH { get; set; }

        [JsonProperty("AVERAGE-ACCOUNT-AGE-YEAR")]
        public int AVERAGEACCOUNTAGEYEAR { get; set; }

        [JsonProperty("AVERAGE-ACCOUNT-AGE-MONTH")]
        public int AVERAGEACCOUNTAGEMONTH { get; set; }

        [JsonProperty("NEW-ACCOUNTS-IN-LAST-SIX-MONTHS")]
        public int NEWACCOUNTSINLASTSIXMONTHS { get; set; }

        [JsonProperty("NEW-DELINQ-ACCOUNT-IN-LAST-SIX-MONTHS")]
        public int NEWDELINQACCOUNTINLASTSIXMONTHS { get; set; }
    }

    public class DRIVINGLICENSEVARIATION
    {
        public string VALUE { get; set; }

        [JsonProperty("REPORTED-DATE")]
        public string REPORTEDDATE { get; set; }
    }

    public class EMAILS
    {
        public List<string> EMAIL { get; set; }
    }

    public class EMAILVARIATION
    {
        public string VALUE { get; set; }

        [JsonProperty("REPORTED-DATE")]
        public string REPORTEDDATE { get; set; }
    }

    public class GROUPDETAILS
    {
        [JsonProperty("GROUP-ID")]
        public string GROUPID { get; set; }

        [JsonProperty("TOT-DISBURSED-AMT")]
        public string TOTDISBURSEDAMT { get; set; }

        [JsonProperty("TOT-CURRENT-BAL")]
        public string TOTCURRENTBAL { get; set; }

        [JsonProperty("TOT-ACCOUNTS")]
        public string TOTACCOUNTS { get; set; }

        [JsonProperty("TOT-DPD-30")]
        public string TOTDPD30 { get; set; }

        [JsonProperty("TOT-DPD-60")]
        public string TOTDPD60 { get; set; }

        [JsonProperty("TOT-DPD-90")]
        public string TOTDPD90 { get; set; }
    }

    public class GRPRESPONSELIST
    {
        public string MFI { get; set; }

        [JsonProperty("MFI-ID")]
        public string MFIID { get; set; }
        public string BRANCH { get; set; }
        public string KENDRA { get; set; }
        public string NAME { get; set; }
        public string DOB { get; set; }
        public string AGE { get; set; }
        public List<ID> IDS { get; set; }
        public PHONES PHONES { get; set; }
        public ADDRESSES ADDRESSES { get; set; }
        public List<RELATION> RELATIONS { get; set; }
        public string CNSMRMBRID { get; set; }

        [JsonProperty("MATCHED-TYPE")]
        public string MATCHEDTYPE { get; set; }

        [JsonProperty("INSERT-DATE")]
        public string INSERTDATE { get; set; }

        [JsonProperty("GROUP-DETAILS")]
        public GROUPDETAILS GROUPDETAILS { get; set; }

        [JsonProperty("LOAN-DETAIL")]
        public LOANDETAIL LOANDETAIL { get; set; }
    }

    public class GRPRESPONSES
    {
        [JsonProperty("PRIMARY-SUMMARY")]
        public PRIMARYSUMMARY PRIMARYSUMMARY { get; set; }

        [JsonProperty("SECONDARY-SUMMARY")]
        public SECONDARYSUMMARY SECONDARYSUMMARY { get; set; }
        public SUMMARY SUMMARY { get; set; }

        [JsonProperty("GRP-RESPONSE-LIST")]
        public List<GRPRESPONSELIST> GRPRESPONSELIST { get; set; }
    }

    public class HEADER
    {
        [JsonProperty("DATE-OF-REQUEST")]
        public string DATEOFREQUEST { get; set; }

        [JsonProperty("PREPARED-FOR")]
        public string PREPAREDFOR { get; set; }

        [JsonProperty("PREPARED-FOR-ID")]
        public string PREPAREDFORID { get; set; }

        [JsonProperty("DATE-OF-ISSUE")]
        public string DATEOFISSUE { get; set; }

        [JsonProperty("BATCH-ID")]
        public string BATCHID { get; set; }

        [JsonProperty("REPORT-ID")]
        public string REPORTID { get; set; }
    }

    public class ID
    {
        public string TYPE { get; set; }
        public string VALUE { get; set; }
    }

    public class INDVREPORT
    {
        [JsonProperty("INDV-REPORT")]
        public INDVREPORT2 INDVREPOR { get; set; }
    }

    public class INDVREPORT2
    {
        public HEADER HEADER { get; set; }
        public REQUEST REQUEST { get; set; }

        [JsonProperty("STATUS-DETAILS")]
        public STATUSDETAILS STATUSDETAILS { get; set; }

        [JsonProperty("PERSONAL-INFO-VARIATION")]
        public PERSONALINFOVARIATION PERSONALINFOVARIATION { get; set; }

        [JsonProperty("SECONDARY-MATCHES")]
        public List<object> SECONDARYMATCHES { get; set; }

        [JsonProperty("ACCOUNTS-SUMMARY")]
        public ACCOUNTSSUMMARY ACCOUNTSSUMMARY { get; set; }
        public List<ALERT> ALERTS { get; set; }
        public List<SCORE> SCORES { get; set; }

        [JsonProperty("INQUIRY-HISTORY")]
        public List<INQUIRYHISTORY> INQUIRYHISTORY { get; set; }
        public List<RESPONSE> RESPONSES { get; set; }

        [JsonProperty("INDV-RESPONSES")]
        public INDVRESPONSES INDVRESPONSES { get; set; }

        [JsonProperty("GRP-RESPONSES")]
        public GRPRESPONSES GRPRESPONSES { get; set; }
    }

    public class INDVREPORTFILE
    {
        [JsonProperty("INDV-REPORTS")]
        public List<INDVREPORT> INDVREPORTS { get; set; }
    }

    public class INDVRESPONSE
    {
        public string reportDt { get; set; }
        public string MFI { get; set; }

        [JsonProperty("MFI-ID")]
        public string MFIID { get; set; }
        public string BRANCH { get; set; }
        public string KENDRA { get; set; }
        public string NAME { get; set; }
        public string DOB { get; set; }
        public string AGE { get; set; }
        public List<ID> IDS { get; set; }
        public PHONES PHONES { get; set; }
        public ADDRESSES ADDRESSES { get; set; }
        public List<RELATION> RELATIONS { get; set; }
        public string CNSMRMBRID { get; set; }

        [JsonProperty("MATCHED-TYPE")]
        public string MATCHEDTYPE { get; set; }

        [JsonProperty("INSERT-DATE")]
        public string INSERTDATE { get; set; }

        [JsonProperty("GROUP-DETAILS")]
        public GROUPDETAILS GROUPDETAILS { get; set; }

        [JsonProperty("LOAN-DETAIL")]
        public LOANDETAIL LOANDETAIL { get; set; }

        [JsonProperty("AGE-AS-ON")]
        public string AGEASON { get; set; }
    }

    public class INDVRESPONSELIST
    {
        [JsonProperty("INDV-RESPONSE")]
        public List<INDVRESPONSE> INDVRESPONSE { get; set; }
    }

    public class INDVRESPONSES
    {
        [JsonProperty("PRIMARY-SUMMARY")]
        public PRIMARYSUMMARY PRIMARYSUMMARY { get; set; }

        [JsonProperty("SECONDARY-SUMMARY")]
        public SECONDARYSUMMARY SECONDARYSUMMARY { get; set; }
        public SUMMARY SUMMARY { get; set; }

        [JsonProperty("INDV-RESPONSE-LIST")]
        public INDVRESPONSELIST INDVRESPONSELIST { get; set; }
    }

    public class INQUIRYHISTORY
    {
        [JsonProperty("MEMBER-NAME")]
        public string MEMBERNAME { get; set; }

        [JsonProperty("INQUIRY-DATE")]
        public string INQUIRYDATE { get; set; }
        public string PURPOSE { get; set; }
        public string AMOUNT { get; set; }
        public string REMARK { get; set; }
    }

    public class LOANDETAIL
    {
        [JsonProperty("ACCT-TYPE")]
        public string ACCTTYPE { get; set; }
        public string FREQ { get; set; }
        public string STATUS { get; set; }

        [JsonProperty("ACCT-NUMBER")]
        public string ACCTNUMBER { get; set; }

        [JsonProperty("DISBURSED-AMT")]
        public string DISBURSEDAMT { get; set; }

        [JsonProperty("CURRENT-BAL")]
        public string CURRENTBAL { get; set; }

        [JsonProperty("INSTALLMENT-AMT")]
        public string INSTALLMENTAMT { get; set; }

        [JsonProperty("OVERDUE-AMT")]
        public string OVERDUEAMT { get; set; }

        [JsonProperty("WRITE-OFF-AMT")]
        public string WRITEOFFAMT { get; set; }

        [JsonProperty("DISBURSED-DT")]
        public string DISBURSEDDT { get; set; }

        [JsonProperty("CLOSED-DT")]
        public string CLOSEDDT { get; set; }

        [JsonProperty("INQ-CNT")]
        public int INQCNT { get; set; }
        public int DPD { get; set; }

        [JsonProperty("INFO-AS-ON")]
        public string INFOASON { get; set; }

        [JsonProperty("COMBINED-PAYMENT-HISTORY")]
        public string COMBINEDPAYMENTHISTORY { get; set; }

        [JsonProperty("LOAN-CYCLE-ID")]
        public int LOANCYCLEID { get; set; }

        [JsonProperty("RECENT-DELINQ-DT")]
        public string RECENTDELINQDT { get; set; }
    }

    public class LOANDETAILS
    {
        [JsonProperty("ACCT-NUMBER")]
        public string ACCTNUMBER { get; set; }

        [JsonProperty("CREDIT-GUARANTOR")]
        public string CREDITGUARANTOR { get; set; }

        [JsonProperty("ACCT-TYPE")]
        public string ACCTTYPE { get; set; }

        [JsonProperty("DATE-REPORTED")]
        public string DATEREPORTED { get; set; }

        [JsonProperty("OWNERSHIP-IND")]
        public string OWNERSHIPIND { get; set; }

        [JsonProperty("ACCOUNT-STATUS")]
        public string ACCOUNTSTATUS { get; set; }

        [JsonProperty("DISBURSED-AMT")]
        public string DISBURSEDAMT { get; set; }

        [JsonProperty("DISBURSED-DATE")]
        public string DISBURSEDDATE { get; set; }

        [JsonProperty("LAST-PAYMENT-DATE")]
        public string LASTPAYMENTDATE { get; set; }

        [JsonProperty("OVERDUE-AMT")]
        public string OVERDUEAMT { get; set; }

        [JsonProperty("WRITE-OFF-AMT")]
        public string WRITEOFFAMT { get; set; }

        [JsonProperty("CURRENT-BAL")]
        public string CURRENTBAL { get; set; }

        [JsonProperty("SECURITY-STATUS")]
        public string SECURITYSTATUS { get; set; }

        [JsonProperty("ORIGINAL-TERM")]
        public string ORIGINALTERM { get; set; }

        [JsonProperty("PRINCIPAL-WRITE-OFF-AMT")]
        public string PRINCIPALWRITEOFFAMT { get; set; }

        [JsonProperty("COMBINED-PAYMENT-HISTORY")]
        public string COMBINEDPAYMENTHISTORY { get; set; }

        [JsonProperty("MATCHED-TYPE")]
        public string MATCHEDTYPE { get; set; }

        [JsonProperty("WRITTEN-OFF_SETTLED-STATUS")]
        public string WRITTENOFF_SETTLEDSTATUS { get; set; }

        [JsonProperty("SETTLEMENT-AMT")]
        public string SETTLEMENTAMT { get; set; }

        [JsonProperty("SECURITY-DETAILS")]
        public List<object> SECURITYDETAILS { get; set; }

        [JsonProperty("LINKED-ACCOUNTS")]
        public List<object> LINKEDACCOUNTS { get; set; }

        [JsonProperty("CREDIT-LIMIT")]
        public string CREDITLIMIT { get; set; }

        [JsonProperty("ACCOUNT-REMARKS")]
        public string ACCOUNTREMARKS { get; set; }
    }

    public class NAMEVARIATION
    {
        public string VALUE { get; set; }

        [JsonProperty("REPORTED-DATE")]
        public string REPORTEDDATE { get; set; }
    }

    public class PANVARIATION
    {
        public string VALUE { get; set; }

        [JsonProperty("REPORTED-DATE")]
        public string REPORTEDDATE { get; set; }
    }

    public class PASSPORTVARIATION
    {
        public string VALUE { get; set; }

        [JsonProperty("REPORTED-DATE")]
        public string REPORTEDDATE { get; set; }
    }

    public class PERSONALINFOVARIATION
    {
        [JsonProperty("NAME-VARIATIONS")]
        public List<NAMEVARIATION> NAMEVARIATIONS { get; set; }

        [JsonProperty("ADDRESS-VARIATIONS")]
        public List<ADDRESSVARIATION> ADDRESSVARIATIONS { get; set; }

        [JsonProperty("PAN-VARIATIONS")]
        public List<PANVARIATION> PANVARIATIONS { get; set; }

        [JsonProperty("DRIVING-LICENSE-VARIATIONS")]
        public List<DRIVINGLICENSEVARIATION> DRIVINGLICENSEVARIATIONS { get; set; }

        [JsonProperty("DATE-OF-BIRTH-VARIATIONS")]
        public List<DATEOFBIRTHVARIATION> DATEOFBIRTHVARIATIONS { get; set; }

        [JsonProperty("VOTER-ID-VARIATIONS")]
        public List<VOTERIDVARIATION> VOTERIDVARIATIONS { get; set; }

        [JsonProperty("PASSPORT-VARIATIONS")]
        public List<PASSPORTVARIATION> PASSPORTVARIATIONS { get; set; }

        [JsonProperty("PHONE-NUMBER-VARIATIONS")]
        public List<PHONENUMBERVARIATION> PHONENUMBERVARIATIONS { get; set; }

        [JsonProperty("RATION-CARD-VARIATIONS")]
        public List<object> RATIONCARDVARIATIONS { get; set; }

        [JsonProperty("EMAIL-VARIATIONS")]
        public List<EMAILVARIATION> EMAILVARIATIONS { get; set; }
    }

    public class PHONENUMBERVARIATION
    {
        public string VALUE { get; set; }

        [JsonProperty("REPORTED-DATE")]
        public string REPORTEDDATE { get; set; }
    }

    public class PHONES
    {
        public List<string> PHONE { get; set; }
    }

    public class PRIMARYACCOUNTSSUMMARY
    {
        [JsonProperty("PRIMARY-NUMBER-OF-ACCOUNTS")]
        public int PRIMARYNUMBEROFACCOUNTS { get; set; }

        [JsonProperty("PRIMARY-ACTIVE-NUMBER-OF-ACCOUNTS")]
        public int PRIMARYACTIVENUMBEROFACCOUNTS { get; set; }

        [JsonProperty("PRIMARY-OVERDUE-NUMBER-OF-ACCOUNTS")]
        public int PRIMARYOVERDUENUMBEROFACCOUNTS { get; set; }

        [JsonProperty("PRIMARY-SECURED-NUMBER-OF-ACCOUNTS")]
        public int PRIMARYSECUREDNUMBEROFACCOUNTS { get; set; }

        [JsonProperty("PRIMARY-UNSECURED-NUMBER-OF-ACCOUNTS")]
        public int PRIMARYUNSECUREDNUMBEROFACCOUNTS { get; set; }

        [JsonProperty("PRIMARY-UNTAGGED-NUMBER-OF-ACCOUNTS")]
        public int PRIMARYUNTAGGEDNUMBEROFACCOUNTS { get; set; }

        [JsonProperty("PRIMARY-CURRENT-BALANCE")]
        public string PRIMARYCURRENTBALANCE { get; set; }

        [JsonProperty("PRIMARY-SANCTIONED-AMOUNT")]
        public string PRIMARYSANCTIONEDAMOUNT { get; set; }

        [JsonProperty("PRIMARY-DISBURSED-AMOUNT")]
        public string PRIMARYDISBURSEDAMOUNT { get; set; }
    }

    public class PRIMARYSUMMARY
    {
        [JsonProperty("NO-OF-DEFAULT-ACCOUNTS")]
        public int NOOFDEFAULTACCOUNTS { get; set; }

        [JsonProperty("TOTAL-RESPONSES")]
        public int TOTALRESPONSES { get; set; }

        [JsonProperty("NO-OF-CLOSED-ACCOUNTS")]
        public int NOOFCLOSEDACCOUNTS { get; set; }

        [JsonProperty("NO-OF-ACTIVE-ACCOUNTS")]
        public int NOOFACTIVEACCOUNTS { get; set; }

        [JsonProperty("NO-OF-OTHER-MFIS")]
        public int NOOFOTHERMFIS { get; set; }

        [JsonProperty("NO-OF-OWN-MFIS")]
        public int NOOFOWNMFIS { get; set; }

        [JsonProperty("TOTAL-OTHER-DISBURSED-AMOUNT")]
        public double TOTALOTHERDISBURSEDAMOUNT { get; set; }

        [JsonProperty("TOTAL-OTHER-CURRENT-BALANCE")]
        public double TOTALOTHERCURRENTBALANCE { get; set; }

        [JsonProperty("TOTAL-OTHER-INSTALLMENT-AMOUNT")]
        public double TOTALOTHERINSTALLMENTAMOUNT { get; set; }

        [JsonProperty("MAX-WORST-DELEQUENCY")]
        public string MAXWORSTDELEQUENCY { get; set; }
    }

    public class RELATION
    {
        public string TYPE { get; set; }
        public string NAME { get; set; }
    }

    public class REQUEST
    {
        public string NAME { get; set; }
        public string AKA { get; set; }
        public string DOB { get; set; }

        [JsonProperty("AGE-AS-ON")]
        public string AGEASON { get; set; }
        public List<ID> IDS { get; set; }
        public ADDRESSES ADDRESSES { get; set; }
        public PHONES PHONES { get; set; }
        public EMAILS EMAILS { get; set; }
        public string GENDER { get; set; }
        public string BRANCH { get; set; }
        public string KENDRA { get; set; }

        [JsonProperty("MBR-ID")]
        public string MBRID { get; set; }

        [JsonProperty("CREDIT-INQ-PURPS-TYP")]
        public string CREDITINQPURPSTYP { get; set; }

        [JsonProperty("CREDIT-INQ-PURPS-TYP-DESC")]
        public string CREDITINQPURPSTYPDESC { get; set; }

        [JsonProperty("CREDIT-INQUIRY-STAGE")]
        public string CREDITINQUIRYSTAGE { get; set; }

        [JsonProperty("CREDIT-RPT-ID")]
        public string CREDITRPTID { get; set; }

        [JsonProperty("CREDIT-REQ-TYP")]
        public string CREDITREQTYP { get; set; }

        [JsonProperty("CREDIT-RPT-TRN-DT-TM")]
        public string CREDITRPTTRNDTTM { get; set; }

        [JsonProperty("LOS-APP-ID")]
        public string LOSAPPID { get; set; }

        [JsonProperty("LOAN-AMOUNT")]
        public string LOANAMOUNT { get; set; }

        [JsonProperty("MFI-IND")]
        public string MFIIND { get; set; }

        [JsonProperty("MFI-SCORE")]
        public string MFISCORE { get; set; }

        [JsonProperty("MFI-GROUP")]
        public string MFIGROUP { get; set; }

        [JsonProperty("CNS-IND")]
        public string CNSIND { get; set; }

        [JsonProperty("CNS-SCORE")]
        public string CNSSCORE { get; set; }
        public string IOI { get; set; }
    }

    public class RESPONSE
    {
        [JsonProperty("LOAN-DETAILS")]
        public LOANDETAILS LOANDETAILS { get; set; }

    }



    public class SCORE
    {
        [JsonProperty("SCORE-TYPE")]
        public string SCORETYPE { get; set; }

        [JsonProperty("SCORE-VALUE")]
        public string SCOREVALUE { get; set; }

        [JsonProperty("SCORE-COMMENTS")]
        public string SCORECOMMENTS { get; set; }
    }

    public class SECONDARYACCOUNTSSUMMARY
    {
        [JsonProperty("SECONDARY-NUMBER-OF-ACCOUNTS")]
        public int SECONDARYNUMBEROFACCOUNTS { get; set; }

        [JsonProperty("SECONDARY-ACTIVE-NUMBER-OF-ACCOUNTS")]
        public int SECONDARYACTIVENUMBEROFACCOUNTS { get; set; }

        [JsonProperty("SECONDARY-OVERDUE-NUMBER-OF-ACCOUNTS")]
        public int SECONDARYOVERDUENUMBEROFACCOUNTS { get; set; }

        [JsonProperty("SECONDARY-SECURED-NUMBER-OF-ACCOUNTS")]
        public int SECONDARYSECUREDNUMBEROFACCOUNTS { get; set; }

        [JsonProperty("SECONDARY-UNSECURED-NUMBER-OF-ACCOUNTS")]
        public int SECONDARYUNSECUREDNUMBEROFACCOUNTS { get; set; }

        [JsonProperty("SECONDARY-UNTAGGED-NUMBER-OF-ACCOUNTS")]
        public int SECONDARYUNTAGGEDNUMBEROFACCOUNTS { get; set; }

        [JsonProperty("SECONDARY-CURRENT-BALANCE")]
        public string SECONDARYCURRENTBALANCE { get; set; }

        [JsonProperty("SECONDARY-SANCTIONED-AMOUNT")]
        public string SECONDARYSANCTIONEDAMOUNT { get; set; }

        [JsonProperty("SECONDARY-DISBURSED-AMOUNT")]
        public string SECONDARYDISBURSEDAMOUNT { get; set; }
    }

    public class SECONDARYSUMMARY
    {
        [JsonProperty("NO-OF-DEFAULT-ACCOUNTS")]
        public int NOOFDEFAULTACCOUNTS { get; set; }

        [JsonProperty("TOTAL-RESPONSES")]
        public int TOTALRESPONSES { get; set; }

        [JsonProperty("NO-OF-CLOSED-ACCOUNTS")]
        public int NOOFCLOSEDACCOUNTS { get; set; }

        [JsonProperty("NO-OF-ACTIVE-ACCOUNTS")]
        public int NOOFACTIVEACCOUNTS { get; set; }

        [JsonProperty("NO-OF-OTHER-MFIS")]
        public int NOOFOTHERMFIS { get; set; }

        [JsonProperty("NO-OF-OWN-MFIS")]
        public int NOOFOWNMFIS { get; set; }
        [JsonProperty("TOTAL-OTHER-DISBURSED-AMOUNT")]
        public double TOTALOTHERDISBURSEDAMOUNT { get; set; }

        [JsonProperty("TOTAL-OTHER-CURRENT-BALANCE")]
        public double TOTALOTHERCURRENTBALANCE { get; set; }

        [JsonProperty("TOTAL-OTHER-INSTALLMENT-AMOUNT")]
        public double TOTALOTHERINSTALLMENTAMOUNT { get; set; }
    }

    public class STATUS
    {
        public string OPTION { get; set; }

        [JsonProperty("OPTION-STATUS")]
        public string OPTIONSTATUS { get; set; }
        public List<object> ERRORS { get; set; }
    }

    public class STATUSDETAILS
    {
        public List<STATUS> STATUS { get; set; }
    }

    public class SUMMARY
    {
        public string STATUS { get; set; }

        [JsonProperty("TOTAL-RESPONSES")]
        public int TOTALRESPONSES { get; set; }

        [JsonProperty("NO-OF-OTHER-MFIS")]
        public int NOOFOTHERMFIS { get; set; }

        [JsonProperty("NO-OF-DEFAULT-ACCOUNTS")]
        public int NOOFDEFAULTACCOUNTS { get; set; }

        [JsonProperty("OWN-MFI-INDECATOR")]
        public bool OWNMFIINDECATOR { get; set; }

        [JsonProperty("NO-OF-CLOSED-ACCOUNTS")]
        public int NOOFCLOSEDACCOUNTS { get; set; }

        [JsonProperty("NO-OF-ACTIVE-ACCOUNTS")]
        public int NOOFACTIVEACCOUNTS { get; set; }

        [JsonProperty("NO-OF-OWN-MFIS")]
        public int NOOFOWNMFIS { get; set; }

        [JsonProperty("TOTAL-OTHER-DISBURSED-AMOUNT")]
        public double TOTALOTHERDISBURSEDAMOUNT { get; set; }

        [JsonProperty("TOTAL-OTHER-CURRENT-BALANCE")]
        public double TOTALOTHERCURRENTBALANCE { get; set; }

        [JsonProperty("TOTAL-OTHER-INSTALLMENT-AMOUNT")]
        public double TOTALOTHERINSTALLMENTAMOUNT { get; set; }

        [JsonProperty("MAX-WORST-DELEQUENCY")]
        public string MAXWORSTDELEQUENCY { get; set; }
    }

    public class VOTERIDVARIATION
    {
        public string VALUE { get; set; }

        [JsonProperty("REPORTED-DATE")]
        public string REPORTEDDATE { get; set; }
    }

}
