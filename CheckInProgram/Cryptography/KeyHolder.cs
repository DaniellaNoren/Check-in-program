using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CheckInProgram.Cryptography
{
    public class KeyHolder
    {
        public static byte[] key { get; set; }
        public static byte[] iv { get; set; }

        public static byte[] entropy { get; set; }

        public static readonly string FILE_PATH = @"./key.txt";

        public static void GenerateKeyIVAndEntropy()
        {
            if (File.Exists(FILE_PATH)) return;

            key = EncryptionHelper.GenerateKey(32);
            iv = EncryptionHelper.GenerateKey(16);
            entropy = EncryptionHelper.GenerateKey(16);

            SaveKeyAndIv(key, iv, entropy);
        }

        public static void ReadKeyAndIv()
        {
            string base64protectedkey = "";
            string base64iv = "";
            string base64entropy = "";

            using (StreamReader sr = new StreamReader(FILE_PATH))
            {
                base64protectedkey = sr.ReadLine().Trim();
                base64entropy = sr.ReadLine().Trim();
                base64iv = sr.ReadLine().Trim();
            }

            byte[] protectedKey = EncryptionHelper.GetByteFromBase64(base64protectedkey);
            entropy = EncryptionHelper.GetByteFromBase64(base64entropy);
            iv = EncryptionHelper.GetByteFromBase64(base64iv);

            key = EncryptionHelper.Unprotect(protectedKey, entropy);
        }

        public static void SaveKeyAndIv(byte[] key, byte[] iv, byte[] entropy)
        {
            byte[] protectedKey = EncryptionHelper.Protect(key, entropy);

            string base64key = EncryptionHelper.GetBase64String(protectedKey);
            string base64IV = EncryptionHelper.GetBase64String(iv);
            string base64Entropy = EncryptionHelper.GetBase64String(entropy);

            using(StreamWriter sw = new StreamWriter(new FileStream(FILE_PATH, FileMode.OpenOrCreate), Encoding.UTF8))
            {
                sw.WriteLine(base64key);
                sw.WriteLine(base64Entropy);
                sw.WriteLine(base64IV);
            }
        }
    }
}
