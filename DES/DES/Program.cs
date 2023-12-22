using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
namespace DES
{
    class Program 
    {
        public enum MODE
        {
          ENCRYPT,
          DECRYPT

        }
        static void Main(string[] args)
        {
            string message;
            Console.Write("Şifrelenecek Metin: ");
            message = Console.ReadLine();

            var random = new Random();
            byte[] IV = new byte[8];
            random.NextBytes(IV);
            byte[] key = new byte[8];
            random.NextBytes(key);
            byte[] encrypted = DESCrypto(MODE.ENCRYPT, IV, key, Encoding.UTF8.GetBytes(message));
            Console.WriteLine("Şifrelenmiş Metin: " + BitConverter.ToString(encrypted).Replace("-", ""));
            Console.ReadLine();




        }

         static byte[] DESCrypto(MODE mode,byte[] IV,byte[] key, byte[]message)
        {
            using(var des= new DESCryptoServiceProvider())
            {
                des.IV = IV;
                des.Key = key;
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;


                using(var memStream=new MemoryStream())
                {
                    CryptoStream cryptoStream = null;

                    if (mode == MODE.ENCRYPT)
                        cryptoStream = new CryptoStream(memStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    else if (mode == MODE.DECRYPT)
                        cryptoStream = new CryptoStream(memStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    if (cryptoStream == null)
                        return null;
                    cryptoStream.Write(message, 0, message.Length);
                    cryptoStream.FlushFinalBlock();
                    return memStream.ToArray();
                }

            }






        }



    }
}
