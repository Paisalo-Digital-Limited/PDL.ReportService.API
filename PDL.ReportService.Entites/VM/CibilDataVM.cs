using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDL.ReportService.Entites.VM
{
    public class CibilDataVM
    {
        public string? ConsumerName { get; set; }
        public string? DOB { get; set; }
        public string? Gender { get; set; }
        public string? IncomeTaxId { get; set; }

        public string? PassportNumber { get; set; }
        public string? PassportIssueDate { get; set; }
        public string? PassportExpiryDate { get; set; }

        public string? VoterIdNumber { get; set; }

        public string? DrivingLicenseNumber { get; set; }
        public string? DrivingLicenseIssueDate { get; set; }
        public string? DrivingLicenseExpiryDate { get; set; }

        public string? AdditionalId2 { get; set; }

        public string? TelephoneMobile { get; set; }
        public string? TelephoneResidence { get; set; }
        public string? TelephoneOffice { get; set; }
        public string? ExtensionOffice { get; set; }
        public string? TelephoneOther { get; set; }
        public string? ExtensionOther { get; set; }

        public string? EmailId1 { get; set; }
        public string? EmailId2 { get; set; }

        // Address 1
        public string? Address1 { get; set; }
        public string? StateCode1 { get; set; }
        public string? PinCode1 { get; set; }
        public string? AddressCategory1 { get; set; }
        public string? ResidenceCode1 { get; set; }

        // Address 2
        public string? Address2 { get; set; }
        public string? StateCode2 { get; set; }
        public string? PinCode2 { get; set; }
        public string? AddressCategory2 { get; set; }
        public string? ResidenceCode2 { get; set; }

        // Current / New Account Details
        public string? CurrentNewMemberCode { get; set; }
        public string? CurrentNewMemberShortName { get; set; }
        public string? CurrentNewAccountNumber { get; set; }
        public string? AccountType { get; set; }
        public string? OwnershipIndicator { get; set; }
        public string? DateOpenedDisbursed { get; set; }
        public string? DateOfLastPayment { get; set; }
        public string? DateClosed { get; set; }
        public string? DateReported { get; set; }

        public string? HighCreditOrSanctionedAmount { get; set; }
        public string? CurrentBalance { get; set; }
        public string? AmountOverdue { get; set; }
        public string? NumberOfDaysPastDue { get; set; }

        // Old / Previous Account Details
        public string? OldMemberCode { get; set; }
        public string? OldMemberShortName { get; set; }
        public string? OldAccountNumber { get; set; }
        public string? OldAccountType { get; set; }
        public string? OldOwnershipIndicator { get; set; }

        public string? SuitFiledWilfulDefault { get; set; }
        public string? WrittenOffSettledStatus { get; set; }
        public string? AssetClassification { get; set; }

        public string? ValueOfCollateral { get; set; }
        public string? TypeOfCollateral { get; set; }

        public string? CreditLimit { get; set; }
        public string? CashLimit { get; set; }
        public string? RateOfInterest { get; set; }
        public string? RepaymentTenure { get; set; }
        public string? EMIAmount { get; set; }

        public string? WrittenOffAmountTotal { get; set; }
        public string? WrittenOffPrincipalAmount { get; set; }
        public string? SettlementAmount { get; set; }

        public string? PaymentFrequency { get; set; }
        public string? ActualPaymentAmount { get; set; }

        public string? OccupationCode { get; set; }
        public string? Income { get; set; }
        public string? NetGrossIncomeIndicator { get; set; }
        public string? MonthlyAnnualIncomeIndicator { get; set; }

        public int? NoInsDue { get; set; }
        public int? DiffDtClosedDtReport { get; set; }
    }
}
