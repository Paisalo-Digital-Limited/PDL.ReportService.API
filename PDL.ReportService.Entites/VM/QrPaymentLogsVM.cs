public class QrPaymentLogsVM
{
    // 🔹 Transaction Details
    public string? TxnId { get; set; }
    public string? VNO { get; set; }
    public string? CustRef { get; set; }
    public decimal Amount { get; set; }
    public string? TxnStatus { get; set; }

    // 🔹 VPA Details
    public string? PayerVpa { get; set; }
    public string? PayeeVpa { get; set; }
    public string? VirtualVpa { get; set; }

    // 🔹 Date Info
    public DateTime? TxnDateTime { get; set; }
    public DateTime? Creationdate { get; set; }

    // 🔹 FI / Branch Info
    public string? Fname { get; set; }
    public string? Branchname { get; set; }
    public string? GroupCode { get; set; }
    public string? FiSmCode { get; set; }
    public string? QRSmCode { get; set; }

    // 🔹 Accounting Info
    public string? Ahead { get; set; }
    public string? SmCodeStatus { get; set; }

    // 🔹 Derived Flags
    public string? QrEntryFlag { get; set; }
    public string? PaymentMode { get; set; }
}
