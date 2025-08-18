using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml.Export.HtmlExport.StyleCollectors.StyleContracts;
using Renci.SshNet;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;


namespace PDL.ReportService.Logics.Helper
{
    public class Helper
    {
       
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

    }
}
