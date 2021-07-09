using CheckInProgram.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CheckInProgram
{
    public class FileSaver
    {
        //private static AESHelper AESHelper = new AESHelper();

        
        public static bool SaveText(string text, string filePath)
        {
            CheckIfFileExists(filePath);

            byte[] byteText = EncryptionHelper.GetByteFromString(text);
            byte[] encryptedText = EncryptionHelper.Encrypt(byteText, KeyHolder.key, KeyHolder.iv);
            string base64text = EncryptionHelper.GetBase64String(encryptedText);
            using(StreamWriter sw = new StreamWriter(filePath, append: true, Encoding.UTF8))
            {
                sw.WriteLine(base64text);
            }

            return true;
            //CheckIfFileExists(filePath);
            //string[] file = GetAllLinesFromFile(filePath);
            //Array.Resize(ref file, file.Length + 1);
            //file[^1] = text;

            //AESHelper.EncryptStringsToFileWithAes(file, filePath);

            //return true;
        }

        public static bool SaveText(string[] text, string filePath)
        {
            CheckIfFileExists(filePath);

            using (StreamWriter sw = new StreamWriter(filePath, append: true, Encoding.UTF8))
            {
                foreach (string t in text)
                {
                    byte[] encryptedText = EncryptionHelper.Encrypt(EncryptionHelper.GetByteFromString(t), KeyHolder.key, KeyHolder.iv);
                    sw.WriteLine(EncryptionHelper.GetBase64String(encryptedText));
                }
            }

            return true;
            //AESHelper.EncryptStringsToFileWithAes(text, filePath);

            //return true;
        }

        private static void CheckIfFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
               
            }
        }

        public static void DeleteAllFromFile(string filePath)
        {
            using (File.Create(filePath))
            {
            }

        }

        public static string[] GetAllLinesFromFile(string filePath)
        {
            CheckIfFileExists(filePath);
            string[] text = File.ReadAllLines(filePath, Encoding.UTF8);

            for(int i = 0; i < text.Length; i++)
            {
                text[i] = EncryptionHelper.GetStringFromByte(EncryptionHelper.Decrypt(EncryptionHelper.GetByteFromBase64(text[i]), KeyHolder.key, KeyHolder.iv));
            }

            return text;
        }

        public static string GetLineFromFile(string searchString, string filePath)
        {
            string foundstr = "";

            using(StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                string s = "";

                while((s = sr.ReadLine()) != null)
                {
                    byte[] encryptedStr = EncryptionHelper.GetByteFromBase64(s);
                    string decryptedStr = EncryptionHelper.GetStringFromByte(EncryptionHelper.Decrypt(encryptedStr, KeyHolder.key, KeyHolder.iv));

                    if (decryptedStr.Contains(searchString))
                    {
                        foundstr = decryptedStr;
                        break;
                    }
                }
            }

            return foundstr;
            //List<string> decryptedLines = AESHelper.DecryptFileWithAes(filePath);
            //return decryptedLines.FirstOrDefault(str => str.Contains(searchString));
        }
        public static string[] GetLinesFromFile(string searchString, string filePath)
        {
            List<string> foundStrings = new List<string>();

            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                string s = "";

                while ((s = sr.ReadLine()) != null)
                {
                    byte[] encryptedStr = EncryptionHelper.GetByteFromBase64(s);
                    string decryptedStr = EncryptionHelper.GetStringFromByte(EncryptionHelper.Decrypt(encryptedStr, KeyHolder.key, KeyHolder.iv));

                    if (decryptedStr.Contains(searchString))
                    {
                        foundStrings.Add(decryptedStr);
                    }
                }
            }

            return foundStrings.ToArray();
        
        }

        public static void ReplaceLine(string searchString, string replacementString, string filePath)
        {
            
            using (StreamWriter sw = new StreamWriter(filePath, append: false, Encoding.UTF8))
            {
                using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    string s = "";

                    while((s = sr.ReadLine()) != null)
                    {
                        byte[] encryptedLine = EncryptionHelper.GetByteFromBase64(s);
                        string decryptedLine = EncryptionHelper.GetStringFromByte(EncryptionHelper.Decrypt(encryptedLine, KeyHolder.key, KeyHolder.iv));

                        if (decryptedLine.Contains(searchString))
                        {
                            encryptedLine = EncryptionHelper.GetByteFromString(decryptedLine);
                            string base64 = EncryptionHelper.GetBase64String(encryptedLine);
                            sw.WriteLine(base64);
                        }
                        else
                        {
                            sw.WriteLine(s);
                        }
                    }
                }
            }
            //    List<string> strings = AESHelper.DecryptFileWithAes(filePath);

            //string[] newStrings = strings.Select(str =>
            //{
            //    if (str.Contains(searchString))
            //        return replacementString;
            //    return str;
            //}).ToArray();

            //AESHelper.EncryptStringsToFileWithAes(newStrings, filePath);
     
        }
        public static void RemoveLineFromFile(string searchString, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, append: false, Encoding.UTF8))
            {
                using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    string s = "";

                    while ((s = sr.ReadLine()) != null)
                    {
                        byte[] encryptedLine = EncryptionHelper.GetByteFromBase64(s);
                        string decryptedLine = EncryptionHelper.GetStringFromByte(EncryptionHelper.Decrypt(encryptedLine, KeyHolder.key, KeyHolder.iv));

                        if (!decryptedLine.Contains(searchString))
                        {
                            sw.WriteLine(s);
                        }
                      
                    }
                }
            }
            //List<string> strings = AESHelper.DecryptFileWithAes(filePath);
            //string[] newStrings = strings.Where(str => !str.Contains(searchString))
            //    .Select(str => str).ToArray();
            //AESHelper.EncryptStringsToFileWithAes(newStrings, filePath);
        }
    }

}

