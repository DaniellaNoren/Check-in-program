using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CheckInProgram
{
    public class EncryptionHelper
    {

        public static byte[] Protect(byte[] key, byte[] entropy)
        {
            return ProtectedData.Protect(key, entropy, DataProtectionScope.CurrentUser);
        }

        public static byte[] Unprotect(byte[] key, byte[] entropy)
        {
            return ProtectedData.Unprotect(key, entropy, DataProtectionScope.CurrentUser);
           
        }

        public static byte[] GenerateKey(int keySize)
        {
            using (var g = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[keySize];
                g.GetBytes(key);
                return key;
            }
        }

        public static byte[] Encrypt(byte[] msg, byte[] key, byte[] iv)
        {
            using(AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using(MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
                    cs.Write(msg);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
        public static byte[] Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            using(AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using(MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(cipherText);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        public static string GetBase64String(byte[] b)
        {
            return Convert.ToBase64String(b);
        }

        public static byte[] GetByteFromBase64(string b64)
        {
            return Convert.FromBase64String(b64);
        }

        public static string GetStringFromByte(byte[] b)
        {
            return Encoding.UTF8.GetString(b);
        }

        public static byte[] GetByteFromString(string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }
    }
}
