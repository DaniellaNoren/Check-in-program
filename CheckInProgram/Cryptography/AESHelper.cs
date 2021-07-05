using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CheckInProgram
{
    public class AESHelper
    {

        private readonly Aes aes;

        public AESHelper()
        {
            aes = (Aes)SymmetricAlgorithm.Create("AES");
            aes.IV = new byte[16];
            aes.Key = Encoding.UTF8.GetBytes("b14ca5898a4e4133bbce2ea2315a1916");  //NOT SAFE
        }

        public byte[] EncryptStringWithAes(string inputMessage)
        {
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            using (StreamWriter writer = new StreamWriter(cryptoStream))
            {
                writer.Write(inputMessage);
            }

            return memoryStream.ToArray();
        }
    
        public void EncryptStringsToFileWithAes(string[] inputMessages, string filePath)
        {
            FileStream memoryStream = new FileStream(filePath, FileMode.Open);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            using (StreamWriter writer = new StreamWriter(cryptoStream))
            {
                foreach (string msg in inputMessages)
                {
                    writer.WriteLine(msg);
                }
            }
        }

        public string DecryptStringWithAes(string filePath)
        {
            FileStream memoryStream = new FileStream(filePath, FileMode.OpenOrCreate);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            
            using (StreamReader reader = new StreamReader(cryptoStream))
            {
                return reader.ReadToEnd();
            }

        }  

        public List<string> DecryptFileWithAes(string filePath)
        {
           
            List<string> strings = new List<string>();

            if (string.IsNullOrEmpty(File.ReadAllText(filePath)))
            {
                return strings;
            }

            FileStream memoryStream = new FileStream(filePath, FileMode.Open);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            string s;
            using (StreamReader reader = new StreamReader(cryptoStream))
            {
                    while(!string.IsNullOrEmpty(s = reader.ReadLine()))
                    {
                        strings.Add(s);
                    }   
            }

            return strings;

        }
    }
}
