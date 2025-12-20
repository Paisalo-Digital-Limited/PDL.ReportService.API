using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml.Export.HtmlExport.StyleCollectors.StyleContracts;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using PDL.ReportService.Entites.VM;
using Renci.SshNet;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;


namespace PDL.ReportService.Logics.Helper
{
    public class Helper
    {
        private static readonly HttpClient _client = new HttpClient();

        public static string Encrypt(string clearText, string key)
        {
            try
            {
                string EncryptionKey = key;
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string Decrypt(string cipherText, string key)
        {
            try
            {
                string EncryptionKey = key;
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public static string GetDBName(IConfiguration configuration)
        {
            string val = configuration["encryptSalts:dbval"];
            string salt = configuration["encryptSalts:dbName"];
            val = Helper.Decrypt(val, salt);
            return val;
        }

        public static void EnsureDirectoryExists(SftpClient sftp, string path)
        {
            string[] directories = path.Split('/');
            string currentPath = "";
            foreach (string dir in directories)
            {
                if (string.IsNullOrEmpty(dir)) continue; // Skip empty parts
                currentPath += $"/{dir}";
                if (!sftp.Exists(currentPath))
                {
                    sftp.CreateDirectory(currentPath);
                    Console.WriteLine($"Created directory: {currentPath}");
                }
            }
        }
        public static List<string> ReadExcelFileToSMCodeList(IFormFile file)
        {
            List<string> smCodes = new List<string>();

            try
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    stream.Position = 0;

                    XSSFWorkbook workbook = new XSSFWorkbook(stream);
                    ISheet sheet = workbook.GetSheetAt(0);

                    if (sheet == null) return smCodes;

                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    // Find SMCode column index
                    int smCodeColIndex = -1;
                    for (int j = 0; j < cellCount; j++)
                    {
                        var cellValue = headerRow.GetCell(j)?.ToString().Trim();
                        if (!string.IsNullOrEmpty(cellValue) && cellValue.Equals("SMCode", StringComparison.OrdinalIgnoreCase))
                        {
                            smCodeColIndex = j;
                            break;
                        }
                    }

                    if (smCodeColIndex == -1) return smCodes;  // SMCode column not found

                    // Read SMCode values
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.Cells.All(d => d == null || d.CellType == CellType.Blank)) continue;

                        ICell smCodeCell = row.GetCell(smCodeColIndex);
                        if (smCodeCell != null)
                        {
                            string smCodeValue = smCodeCell.ToString().Trim();
                            if (!string.IsNullOrEmpty(smCodeValue))
                                smCodes.Add(smCodeValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading Excel file: " + ex.Message);
            }

            return smCodes;
        }
        public static async Task<APIResponseVM> SaveRcManualByExcel(ICICIRcPostManualVM postData, string activeUser, Dictionary<string, string> allUrl, string token)
        {
            APIResponseVM apiResponse = new APIResponseVM();
            try
            {
                token = "";
                if (postData == null)
                    throw new Exception("This Case not Found in system");

                string collCashUrl = allUrl["collCashUrl"];
                string dbName = allUrl["dbname"];

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("dbname", dbName);
                    client.DefaultRequestHeaders.Add("userid", activeUser);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    string json = JsonConvert.SerializeObject(postData);
                    using var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _client.PostAsync(collCashUrl, content);
                    apiResponse.StatusCode = response.StatusCode;
                    apiResponse.IsSuccessStatusCode = response.IsSuccessStatusCode;
                    apiResponse.ReasonPhase = response.ReasonPhrase;
                    apiResponse.ResponseContent =await response.Content.ReadAsStringAsync();

                }
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccessStatusCode = false;
                apiResponse.ResponseContent = "Exception: " + ex.Message;
            }
            return apiResponse;
        }
        public static List<IciciExcelFileVM> ReadIciciExcelFile(string filePath)
        {
            var list = new List<IciciExcelFileVM>();

            try
            {
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                XSSFWorkbook workbook = new(stream);
                ISheet sheet = workbook.GetSheetAt(0);

                // Map header columns
                IRow headerRow = sheet.GetRow(sheet.FirstRowNum);
                var columnMapping = new Dictionary<string, int>();
                for (int i = headerRow.FirstCellNum; i < headerRow.LastCellNum; i++)
                {
                    var cellValue = headerRow.GetCell(i)?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(cellValue))
                        columnMapping[cellValue] = i;
                }

                // Read data rows
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue;

                    var vm = new IciciExcelFileVM
                    {
                        BankRRN = columnMapping.ContainsKey("BankRRN") ? GetCellValue(row.GetCell(columnMapping["BankRRN"])) : null,
                        MerchantTranId = columnMapping.ContainsKey("MerchantTranId") ? GetCellValue(row.GetCell(columnMapping["MerchantTranId"])) : null,
                        PayerVA = columnMapping.ContainsKey("PayerVA") ? GetCellValue(row.GetCell(columnMapping["PayerVA"])) : null,
                        PayerAccountType = columnMapping.ContainsKey("PayerAccountType") ? GetCellValue(row.GetCell(columnMapping["PayerAccountType"])) : null,
                        PayerAmount = columnMapping.ContainsKey("PayerAmount") && decimal.TryParse(GetCellValue(row.GetCell(columnMapping["PayerAmount"])), out var amt) ? amt : (decimal?)null,
                        TerminalId = columnMapping.ContainsKey("TerminalId") ? GetCellValue(row.GetCell(columnMapping["TerminalId"])) : null,
                        SubMerchantId = columnMapping.ContainsKey("SubMerchantId") ? GetCellValue(row.GetCell(columnMapping["SubMerchantId"])) : null,
                        SeqNo = columnMapping.ContainsKey("SeqNo") ? GetCellValue(row.GetCell(columnMapping["SeqNo"])) : null,
                        Date = columnMapping.ContainsKey("Date") ? GetDateTimeCell(row.GetCell(columnMapping["Date"])) : null
                    };

                    list.Add(vm);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading Excel file: " + ex.Message, ex);
            }

            return list;
        }

        // Convert any cell to string
        public static string GetCellValue(ICell cell)
        {
            if (cell == null) return null;

            return cell.CellType switch
            {
                CellType.String => cell.StringCellValue?.Trim(),

                CellType.Numeric => DateUtil.IsCellDateFormatted(cell)
                    ? ((DateTime)cell.DateCellValue).ToString("yyyy-MM-dd HH:mm:ss")
                    : cell.NumericCellValue.ToString(),

                CellType.Boolean => cell.BooleanCellValue.ToString(),

                CellType.Formula => cell.ToString().Trim(),

                _ => cell.ToString().Trim()
            };
        }

        // Convert any cell to nullable DateTime
        public static DateTime? GetDateTimeCell(ICell cell)
        {
            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
                return null;

            if (cell.CellType == CellType.Numeric && DateUtil.IsCellDateFormatted(cell))
                return cell.DateCellValue; // preserves time
            string text = cell.ToString().Trim();

            string[] formats ={"MM-dd-yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "dd-MM-yyyy", "dd/MM/yyyy"};

            if (DateTime.TryParseExact( text,formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }

            return null;
        }
        public static string GetErrorMessage(int code)
        {
            if (code == 0) return "Transaction already exists";
            if (code == -1) return "Invalid BankRRN or MerchantTranId";
            if (code == -2) return "Invalid transaction date";
            if (code == -3) return "ICICI callback insert failed";
            if (code == -4) return "QR payment insert failed";
            if (code == -5) return "RC payload preparation failed";
            if (code == -7) return "RC Post process failed";

            return "File upload failed";
        }

    }
}
