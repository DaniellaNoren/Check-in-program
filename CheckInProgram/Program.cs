﻿using CheckInProgram.Persists;
using System;
using System.Collections.Generic;
using System.IO;

namespace CheckInProgram
{
    class Program
    {
        private bool LOGGED_IN;
        private bool CONTINUE_PROGRAM = true;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.MenuLoop();
        }

        public static void InitiateUsers()
        {
            string[] jsonStrings = ObjectParser.GetJsonsFromObjects(new User[] { new User("daniella", "password"), new User("joppe", "poppe") });
            try
            {
                FileSaver.SaveText(jsonStrings, filePath);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File not found, cannot create users");
            }
        }
        public void MenuLoop()
        {
            while (CONTINUE_PROGRAM)
            {
                if (!LOGGED_IN)
                    Console.WriteLine("1. Login\n2. Create user\n3. Exit");
                else
                    Console.WriteLine("1. Print something funny\n2. View all users\n3. Log out");

                int choice = GetNumberInput();
                Choose(choice);
            }
        }

        public void Choose(int choice)
        {
            if (!LOGGED_IN)
            {
                switch (choice)
                {
                    case 1: TryToLogin(); break;
                    case 2: CreateUser(); break;
                    case 3: Console.WriteLine("Bye!"); CONTINUE_PROGRAM = false; break;
                    default: break;
                }
            }
            else
            {
                switch (choice)
                {
                    case 1: Console.WriteLine("Something funny!"); break;
                    case 2: ViewAllUsers(); break;
                    case 3: Console.WriteLine("Logging out... "); LOGGED_IN = false; break;
                    default: break;
                }
            }
        }

        private void ViewAllUsers()
        {
            List<User> users = Persister.GetObjects();
            foreach (User user in users)
            {
                Console.WriteLine(user);
            }
        }
        private static IPersister<User> Persister = new FileUserPersister();
        public User CreateUser()
        {
            string userName = GetInput("Username");
            string password = GetInput("Password");
            string password2 = GetInput("Confirm password");

            if (password.Equals(password2))
            {
                User user = new User(userName, password);
                Persister.SaveObject(user);
                return user;
            }

            throw new Exception();
        }
        public void TryToLogin()
        {
            string userName = GetInput("Username");
            string password = GetInput("Password");

            LOGGED_IN = Login.TryLogin(userName, password);

            if (!LOGGED_IN)
                Console.WriteLine("Login failed.");
        }
        public string GetInput(string command)
        {
            Console.WriteLine($"{command}:");
            return Console.ReadLine();
        }

        public int GetNumberInput()
        {
            return Int32.Parse(Console.ReadLine().Trim());
        }
    }
}