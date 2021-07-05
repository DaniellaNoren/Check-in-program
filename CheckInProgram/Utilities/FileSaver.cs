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
        private static AESHelper AESHelper = new AESHelper();
        public static bool SaveText(string text, string filePath)
        {
            CheckIfFileExists(filePath);
            string[] file = GetAllLinesFromFile(filePath);
            Array.Resize(ref file, file.Length + 1);
            file[^1] = text;

            AESHelper.EncryptStringsToFileWithAes(file, filePath);

            return true;
        }

        public static bool SaveText(string[] text, string filePath)
        {
            CheckIfFileExists(filePath);

            AESHelper.EncryptStringsToFileWithAes(text, filePath);

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
            return AESHelper.DecryptFileWithAes(filePath).ToArray();
        }

        public static string GetLineFromFile(string searchString, string filePath)
        {
            List<string> decryptedLines = AESHelper.DecryptFileWithAes(filePath);
            return decryptedLines.FirstOrDefault(str => str.Contains(searchString));
        }
        public static string[] GetLinesFromFile(string searchString, string filePath)
        {
            List<string> strings = AESHelper.DecryptFileWithAes(filePath);

            return strings.Where(str => str.Contains(searchString)).Select(str => str).ToArray();
        
        }

        public static void ReplaceLine(string searchString, string replacementString, string filePath)
        {
            List<string> strings = AESHelper.DecryptFileWithAes(filePath);

            string[] newStrings = strings.Select(str =>
            {
                if (str.Contains(searchString))
                    return replacementString;
                return str;
            }).ToArray();

            AESHelper.EncryptStringsToFileWithAes(newStrings, filePath);
     
        }
        public static void RemoveLineFromFile(string searchString, string filePath)
        {
            List<string> strings = AESHelper.DecryptFileWithAes(filePath);
            string[] newStrings = strings.Where(str => !str.Contains(searchString))
                .Select(str => str).ToArray();
            AESHelper.EncryptStringsToFileWithAes(newStrings, filePath);
        }
    }

}

