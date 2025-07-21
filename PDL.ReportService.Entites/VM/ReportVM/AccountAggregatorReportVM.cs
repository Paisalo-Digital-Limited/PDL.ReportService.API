using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{

    public class StandardResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class Profile
    {
        public Holders holders { get; set; }
    }

    public class Holders
    {
        public holder holder { get; set; }
        public string type { get; set; }
    }

    public class holder
    {
        public string Name { get; set; }
        public DateTime? DOB { get; set; }
        public string mobile { get; set; }
        public string nominee { get; set; }
        public string landline { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public string ckycComplienceTrue { get; set; }
    }

    public class JsonData
    {
        public string accountId { get; set; }
        public JsonDataInner data { get; set; }
        public string status { get; set; }  // Example: "READY"
    }

    public class JsonDataInner
    {
        public Profile profile { get; set; }
        public summary summary { get; set; }
        public TransactionsContainer transactions { get; set; }
        public string type { get; set; }
        public string maskedAccNumber { get; set; }
        public string version { get; set; }
        public string linkedAccRef { get; set; }
    }


    public class summary
    {
        public string branch { get; set; }
        public string facility { get; set; }
        public string ifscCode { get; set; }
        public string micrCode { get; set; }
        public string openingDate { get; set; }
        public string currentODLimit { get; set; }
        public string drawingLimit { get; set; }
        public string status { get; set; }
        public pending pending { get; set; }
        public string currentBalance { get; set; }
        public string currency { get; set; }
        public string exchangeRate { get; set; }
        public string balanceDateTime { get; set; }
        public string type { get; set; }
    }

    public class pending
    {
        public string transactionType { get; set; }
        public string amount { get; set; }
    }
    public class TransactionsContainer
    {
        public List<TransactionsVM> transaction { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class TransactionsVM : Transaction
    {
    }

    public class Transaction
    {
        public string TxnID { get; set; }
        public string Amount { get; set; }
        public string Narration { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
        public string CurrentBalance { get; set; }
        public string transactionTimestamp { get; set; }
        public string ValueDate { get; set; }
        public string Reference { get; set; }
    }
    public class data
    {
        public List<JsonData> JsonData { get; set; }
        public CrifFormattedData Data { get; set; }
    }

    public class CrifFormattedData
    {
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonProperty("data")]
        public CrifFormattedDataInner InnerData { get; set; }
        public string ReferenceId { get; set; }
        public string TrackingId { get; set; }
        public string AnalysisId { get; set; }
    }
    public class CrifFormattedDataInner
    {
        [JsonProperty("analytics")]
        public AnalyticsRoot analytics { get; set; }

        [JsonProperty("categorizedTransactions")]
        public List<CategorizedTransactionsContainer> categorizedTransactions { get; set; }
    }

    public class AnalyticsRoot
    {
        public ConsumerVM Consumer { get; set; }
    }

    public class CategorizedTransactionsContainer
    {
        public string accountId { get; set; }
        public List<CategorizedTransaction> transactions { get; set; }
    }

    public class CategorizedTransaction
    {
        public string transactionId { get; set; }
        public string status { get; set; }
        public string bookingDate { get; set; }
        public string valueDate { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public balance balance { get; set; }
    }
    public class balance
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class ConsumerVM
    {
        public ConsumerBase Base { get; set; }
        public IdentityVM Identity { get; set; }
        public CashFlowVM CashFlow { get; set; }
        public Score score { get; set; }
        public Risk risk { get; set; }
        public Marketing marketing { get; set; }
        public IncomeEstimation incomeEstimation { get; set; }
    }
    public class IncomeEstimation
    {
        public string estimation { get; set; }
        public string projection { get; set; }
    }
    public class Marketing
    {
        public string house { get; set; }
        public string loan { get; set; }
        public string travelling { get; set; }
        public string digital { get; set; }
        public string tax { get; set; }
    }

    public class Risk
    {
        public string indebtedness { get; set; }
        public string gamblingGames { get; set; }
    }

    public class Score
    {
        public string transactionScore { get; set; }
        public string area { get; set; }
        public string tranche { get; set; }
        public string factor { get; set; }
    }
    public class ConsumerBase
    {
        public SubjectVM Subject { get; set; }
    }

    public class SubjectVM
    {
        public string SubjectId { get; set; }
        public DataPeriodVM DataPeriod { get; set; }
        public KPI_VM KPI { get; set; }
        public List<ConnectionVM> Connections { get; set; }
    }

    public class DataPeriodVM
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysCount { get; set; }
        public int FullMonthCount { get; set; }
    }

    public class KPI_VM
    {
        public BalanceVM Balance { get; set; }
        public TransactionCountVM PeriodTransactionsCount { get; set; }
        public TransactionCountVM MonthlyTransactionsCount { get; set; }
    }

    public class BalanceVM
    {
        public double totalBalanceAmount { get; set; }
        public double averageBalanceAmount { get; set; }
        public double medianBalanceAmount { get; set; }
        public double minBalanceAmount { get; set; }
        public double maxBalanceAmount { get; set; }
    }

    public class Balance
    {
        public double openingBalanceAmount { get; set; }
        public double closingBalanceAmount { get; set; }
        public double averageBalanceAmount { get; set; }
        public double medianBalanceAmount { get; set; }
        public double minimumBalanceAmount { get; set; }
        public double maximumBalanceAmount { get; set; }
    }

    public class TransactionCountVM
    {
        public int Total { get; set; }
        public int Income { get; set; }
        public int Expenses { get; set; }
    }

    public class ConnectionVM
    {
        public string ConnectionId { get; set; }
        public DataPeriodVM DataPeriod { get; set; }
        public KPI_VM KPI { get; set; }
        public List<AccountVM> Accounts { get; set; }
    }

    public class AccountVM
    {
        public string AccountId { get; set; }
        public WarningVM Warnings { get; set; }
        public DataPeriodVM DataPeriod { get; set; }
        public AccountKpiVM Kpi { get; set; }
    }

    public class WarningVM
    {
        public bool WithoutTransactions { get; set; }
    }

    public class AccountKpiVM : KPI_VM
    {
        public int SignificanceIndex { get; set; }
        public int NewestTransactionDays { get; set; }
        public int OldestTransactionDays { get; set; }
    }

    public class IdentityVM
    {
        public string Verification { get; set; }
        public bool SoleTrader { get; set; }
    }

    public class CashFlowVM
    {
        public List<MonthlyAnalysisVM> MonthlyAnalysis { get; set; }
        public PeriodAnalysisVM PeriodAnalysis { get; set; }
        public InsightsVM Insights { get; set; }
    }

    public class MonthlyAnalysisVM
    {
        public string Month { get; set; }
        public bool FullMonth { get; set; }
        public double IncomeAmount { get; set; }
        public double ExpensesAmount { get; set; }
        public List<CategoryAmountVM> ExpensesByCategory { get; set; }
        public double SavingAmount { get; set; }
        public double SavingRatio { get; set; }
        public double IncomeExpensesRatio { get; set; }
        public OverdraftVM Overdraft { get; set; }
        public Balance Balance { get; set; }
        public List<CategoryAmountVM> IncomeByCategory { get; set; }
    }

    public class PeriodAnalysisVM
    {
        public AmountSummaryVM IncomeAmount { get; set; }
        public List<CategoryDetailVM> IncomeByCategory { get; set; }
        public AmountSummaryVM ExpensesAmount { get; set; }
        public List<CategoryDetailVM> ExpensesByCategory { get; set; }
        public AmountSummaryVM SavingAmount { get; set; }
        public double SavingRatio { get; set; }
        public double IncomeExpensesRatio { get; set; }
        public int MonthsWithNegativeSavingCount { get; set; }
    }

    public class InsightsVM
    {
        public DirectDebitVM BestAccountDirectDebit { get; set; }
        public double ExpensesRunwayMonths { get; set; }
        public double SavingRunwayMonths { get; set; }
        public CurrentMonthVM CurrentMonth { get; set; }
        public List<ForecastVM> Forecast { get; set; }
        public double MonthlyAffordableAmount { get; set; }
    }

    public class CategoryAmountVM
    {
        public string Code { get; set; }
        public double Amount { get; set; }
        public int Count { get; set; }
    }

    public class CategoryDetailVM
    {
        public string Code { get; set; }
        public AmountSummaryVM Amount { get; set; }
        public CountSummaryVM Count { get; set; }
        public OutlierVM Outlier { get; set; }
    }

    public class AmountSummaryVM
    {
        public double Total { get; set; }
        public double Average { get; set; }
        public double Median { get; set; }
        public double Stability { get; set; }
    }

    public class CountSummaryVM
    {
        public int Total { get; set; }
        public double Average { get; set; }
        public double Median { get; set; }
        public double Regularity { get; set; }
    }

    public class ForecastVM
    {
        public string Month { get; set; }
        public double IncomeExpectedAmount { get; set; }
        public double ExpensesExpectedAmount { get; set; }
        public double SavingExpectedAmount { get; set; }
    }

    public class ExpectedActualVM
    {
        public double ExpectedAmount { get; set; }
        public double ActualAmount { get; set; }
    }

    public class CurrentMonthVM
    {
        public string Month { get; set; }
        public ExpectedActualVM Income { get; set; }
        public ExpectedActualVM Expenses { get; set; }
        public ExpectedActualVM Saving { get; set; }
    }

    public class DirectDebitVM
    {
        public string AccountId { get; set; }
        public int Day { get; set; }
        public double Amount { get; set; }
    }

    public class OverdraftVM
    {
        public int BalanceBelowZeroDays { get; set; }
    }

    public class OutlierVM
    {
        public string TransactionId { get; set; }
        public double Deviation { get; set; }
    }
}