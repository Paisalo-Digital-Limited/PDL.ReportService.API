using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM.ReportVM
{
    public class AccountAggregatorReportVM
    {
        public List<BorrowerInfoVM> BorrowerInfo { get; set; }
        public List<TransactionsVM> Transactions { get; set; }
        public AnalyticsVM Analytics { get; set; }
    }

    public class StandardResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class OuterDataWrapper
    {
        public CrifFormattedData Data { get; set; }
    }

    public class CrifFormattedData
    {
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public AnalyticsWrapper Data { get; set; }
        public string ReferenceId { get; set; }
        public string TrackingId { get; set; }
        public string AnalysisId { get; set; }
    }

    public class AnalyticsWrapper
    {
        public AnalyticsRoot Analytics { get; set; }
    }

    public class AnalyticsRoot
    {
        public ConsumerVM Consumer { get; set; }
    }

    public class ConsumerVM
    {
        public ConsumerBase Base { get; set; }
        public IdentityVM Identity { get; set; }
        public CashFlowVM CashFlow { get; set; }

        public string TransactionScore { get; set; }
        public string ScoreArea { get; set; }
        public string ScoreTranche { get; set; }
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
        public double TotalBalanceAmount { get; set; }
        public double AverageBalanceAmount { get; set; }
        public double MedianBalanceAmount { get; set; }
        public double MinBalanceAmount { get; set; }
        public double MaxBalanceAmount { get; set; }
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
        public BalanceVM Balance { get; set; }
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