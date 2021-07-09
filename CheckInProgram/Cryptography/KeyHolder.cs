using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CheckInProgram.Cryptography
{
    public class KeyHolder
    {
        public static byte[] Key { get { return key; } set { key = value; } }
        private static byte[] key;
        public static byte[] IV { get { return iv; } set { iv = value; } }
        private static byte[] iv;
        public static byte[] Entropy { get { return entropy; } set { entropy = value; } }
        private static byte[] entropy;

        public static readonly string FILE_PATH = @"./key.txt";

        public static void GenerateKeyIVAndEntropy()
        {
            if (File.Exists(FILE_PATH)) return;

            Key = EncryptionHelper.GenerateKey(32);
            IV = EncryptionHelper.GenerateKey(16);
            Entropy = EncryptionHelper.GenerateKey(16);

            SaveKeyAndIv(Key, IV, Entropy);
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
            Entropy = EncryptionHelper.GetByteFromBase64(base64entropy);
            IV = EncryptionHelper.GetByteFromBase64(base64iv);

            Key = EncryptionHelper.Unprotect(protectedKey, Entropy);
        }

        public static void SaveKeyAndIv(byte[] key, byte[] iv, byte[] entropy)
        {
            byte[] protectedKey = EncryptionHelper.Protect(key, entropy);

            string base64key = EncryptionHelper.GetBase64String(protectedKey);
            string base64IV = EncryptionHelper.GetBase64String(iv);
            string base64Entropy = EncryptionHelper.GetBase64String(entropy);

            using (StreamWriter sw = new StreamWriter(new FileStream(FILE_PATH, FileMode.OpenOrCreate), Encoding.UTF8))
            {
                sw.WriteLine(base64key);
                sw.WriteLine(base64Entropy);
                sw.WriteLine(base64IV);
            }
        }
    }
}
