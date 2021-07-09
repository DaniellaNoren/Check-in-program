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
            using (StreamWriter sw = new StreamWriter(filePath, append: true, Encoding.UTF8))
            {
                sw.WriteLine(base64text);
            }

            return true;
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

            for (int i = 0; i < text.Length; i++)
            {
                text[i] = EncryptionHelper.GetStringFromByte(EncryptionHelper.Decrypt(EncryptionHelper.GetByteFromBase64(text[i]), KeyHolder.key, KeyHolder.iv));
            }

            return text;
        }

        public static string GetLineFromFile(string searchString, string filePath)
        {
            string foundstr = "";

            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                string s = "";

                while ((s = sr.ReadLine()) != null)
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
            string tempFile = "./temp.txt";

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream fsTemp = new FileStream(tempFile, FileMode.CreateNew, FileAccess.Write, FileShare.Write);

            StreamWriter sw = new StreamWriter(fsTemp, Encoding.UTF8);


            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                string s = "";

                while ((s = sr.ReadLine()) != null)
                {
                    byte[] encryptedLine = EncryptionHelper.GetByteFromBase64(s);
                    string decryptedLine = EncryptionHelper.GetStringFromByte(EncryptionHelper.Decrypt(encryptedLine, KeyHolder.key, KeyHolder.iv));

                    if (decryptedLine.Contains(searchString))
                    {
                        encryptedLine = EncryptionHelper.Encrypt(EncryptionHelper.GetByteFromString(replacementString), KeyHolder.key, KeyHolder.iv);
                        string base64 = EncryptionHelper.GetBase64String(encryptedLine);
                        sw.WriteLine(base64);
                    }
                    else
                    {
                        sw.WriteLine(s);
                    }
                }

            }
           
            sw.Close();
            File.Delete(filePath);
            File.Move(tempFile, filePath);
        }

        public static void RemoveLineFromFile(string searchString, string filePath)
        {
            string tempFile = "./temp.txt";

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            FileStream fsTemp = new FileStream(tempFile, FileMode.CreateNew, FileAccess.Write, FileShare.Write);

            StreamReader sr = new StreamReader(fs, Encoding.UTF8);

            using (StreamWriter sw = new StreamWriter(fsTemp, Encoding.UTF8))
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
            
            sr.Close();
            File.Delete(filePath);
            File.Move(tempFile, filePath);

        }
    }

}

