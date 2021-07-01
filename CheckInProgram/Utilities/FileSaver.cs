using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CheckInProgram
{
    public class FileSaver
    {
        public static bool SaveText(string text, string filePath)
        {
            CheckIfFileExists(filePath);

            using (StreamWriter sw = new StreamWriter(filePath, append: true))
            {
                sw.WriteLine(text);
            }

            return true;
        }

        private static void CheckIfFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.CreateNew))
                {

                }
            }
        }
        public static bool SaveText(string[] text, string filePath)
        {
            CheckIfFileExists(filePath);

            using (StreamWriter sw = new StreamWriter(filePath, append: true))
            {
                foreach (string t in text)
                {
                    sw.WriteLine(t);
                }
            }

            return true;
        }

        public static void DeleteAllFromFile(string filePath)
        {
            using (File.Create(filePath))
            {
            }

        }
        public static void ReadFile(string filePath)
        {
            string s;
            using (StreamReader sr = new StreamReader(filePath))
            {
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }

        public static string[] GetAllLinesFromFile(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
        public static string GetLineFromFile(string searchString, string filePath)
        {
            string s = "";
            using (StreamReader sr = new StreamReader(filePath))
            {
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Contains(searchString))
                    {
                        sr.Dispose();
                        return s;
                    }
                }
            }

            return s;
        } 
        public static string[] GetLinesFromFile(string searchString, string filePath)
        {
            string[] strings = new string[0];
            int count = 0;
            string s = "";
            using (StreamReader sr = new StreamReader(filePath))
            {
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Contains(searchString))
                    {
                        Array.Resize(ref strings, strings.Length + 1);
                        strings[count] = s;
                        count++;
                    }
                    
                }
            }

            return strings;
        }
        public static void ReplaceLine(string searchString, string replacementString, string filepath)
        {
            string[] lines = GetAllLinesFromFile(filepath);

            using (StreamWriter sw = new StreamWriter(filepath, append: false))
            {
                foreach (string line in lines)
                {
                    if (line.Contains(searchString))
                    {
                        sw.WriteLine(replacementString);
                    }
                    else
                        sw.WriteLine(line.Trim());
                }
            }
        }
        public static void RemoveLineFromFile(string searchString, string filePath)
        {

            string[] tempFile = File.ReadAllText(filePath).Split('\n');

            using (StreamWriter sw = new StreamWriter(filePath, append: false))
            {
                for (int i = 0; i < tempFile.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(tempFile[i]) && !tempFile[i].Contains(searchString))
                    {
                        sw.WriteLine(tempFile[i].Trim());
                    }

                }
            }

        }
    }

}

