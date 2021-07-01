using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CheckInProgram
{
    public class InputHandler
    {
        public static int GetInt()
        {
            string input = GetInput();

            if (Regex.IsMatch(input, @"^\d*$"))
                return Int32.Parse(input);

            throw new InvalidInputException("Invalid number");
            
        }

       
        public static double GetDouble()
        {
            string input = GetInput();

            if (Regex.IsMatch(input, @"^\d*(?:.\d*)+$"))
                return Int32.Parse(input);

            throw new InvalidInputException("Invalid number");
        }

        public static string GetString()
        {
            return GetInput();
        }

        public static string GetString(string pattern)
        {
            string input = GetInput();

            if (Regex.IsMatch(input, pattern))
                return input;

            throw new InvalidInputException("Input did not match pattern");
        }

        private static string GetInput()
        {
            try
            {
                string input = Console.ReadLine();
                return input;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }
    }

    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message)
        {

        }
    }
}
