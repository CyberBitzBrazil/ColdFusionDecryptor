using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ColdFusionCracker
{
    public class UserPasswords
    {
        public string Email { get; set; }
        public string PasswordRaw { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordDecrpted
        {
            get { return DecryptPassword(PasswordRaw, PasswordHash); }
        }
 

        private string DecryptPassword(string passwordString, string passwordKey)
        {
            string result;
            try
            {
                byte[] bytes = Convert.FromBase64String(passwordString);
                byte[] key = Convert.FromBase64String(passwordKey);

                string decryptedText = null;
                using (RijndaelManaged algorithm = new())
                {
                    algorithm.Mode = CipherMode.ECB;
                    algorithm.Padding = PaddingMode.PKCS7;
                    algorithm.BlockSize = 128;
                    algorithm.KeySize = 128;
                    algorithm.Key = key;

                    ICryptoTransform decryptor = algorithm.CreateDecryptor();

                    using MemoryStream msDecrypt = new(bytes);
                    using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                    using StreamReader srDecrypt = new(csDecrypt);
                    decryptedText = srDecrypt.ReadToEnd();
                }
                result = decryptedText;
            }
            catch
            {
                result = "ERROR";
            }
            return result;
        }
    }

 
}
