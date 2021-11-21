using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CancelationTokenTask.Encrypttion
{
   public class EncryptDecrypt
    {
       public static void FileStreamWrite(string text, string filepath)
        {
            try
            {
                using (FileStream fs = new FileStream($" {filepath}", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {

                    byte[] bytes = Encoding.UTF8.GetBytes(text);
                    fs.Write(bytes, 0, bytes.Length);

                }

            }
            catch (Exception)
            {


            }



        }

        public static void FileStreamRead(string text, string filepath)
        {

            try
            {
                using (FileStream fs = new FileStream($"{filepath}", FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
                {
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    text = Encoding.UTF8.GetString(bytes);

                }

            }
            catch
            {

            }

        }

        public static string EString(string key, string text)
        {

            TripleDESCryptoServiceProvider TDC = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] byteHash, byteText;

            byteHash = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            byteText = UTF8Encoding.UTF8.GetBytes(text);

            md5.Clear();

            TDC.Key = byteHash;
            TDC.Mode = CipherMode.ECB;

            return Convert.ToBase64String(TDC.CreateEncryptor().TransformFinalBlock(byteText, 0, byteText.Length));

        }

        public static string DString(string key, string text)
        {

            TripleDESCryptoServiceProvider TDC = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] byteHash, byteText;

            byteHash = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            byteText = Convert.FromBase64String(text);

            md5.Clear();

            TDC.Key = byteHash;
            TDC.Mode = CipherMode.ECB;

            return UnicodeEncoding.UTF8.GetString(TDC.CreateDecryptor().TransformFinalBlock(byteText, 0, byteText.Length));


        }
    }
}
