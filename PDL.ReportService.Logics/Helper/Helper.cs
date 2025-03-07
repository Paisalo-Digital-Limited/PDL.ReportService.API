using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using Renci.SshNet;

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
    }
}
