using CheckInProgram.Persists;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CheckInProgram
{
    class CheckInProgram //TODO: Handle input exceptions
    {
        private User LOGGED_IN_USER;
        private bool LOGGED_IN;
        private bool CONTINUE_PROGRAM = true;

        private static readonly IPersister<TimeStamp> timeStampPersister = new FileTimeStampPersister();
        private static IPersister<User> userPersister = new FileUserPersister();

        static void Main(string[] args)
        {
            CheckInProgram program = new CheckInProgram();
            program.MenuLoop();
        }

        public void MenuLoop()
        {
            while (CONTINUE_PROGRAM)
            {
                ChangeTextColor(ConsoleColor.Blue);

                if (!LOGGED_IN)
                    Console.WriteLine("1. Login\n2. Create user\n3. Exit");
                else
                    Console.WriteLine("1. Print something funny\n2. View all users\n3. View timestamps\n4. Log out");

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
                    case 3: ViewTimeSpans(); break;
                    case 4: CheckOut(); break;
                    default: break;
                }
            }
        }

        private void LoopThroughList<T>(List<T> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        private void ViewUserTimeStamps(string userName)
        {

            List<TimeStamp> timeStamps = timeStampPersister.GetObjects($"\"User\":{{\"UserName\":\"{userName}\"");
            LoopThroughList(timeStamps);

        }   
        private void ViewUserTimeStamps()
        {
            Console.WriteLine("Which user's timestamps do you want to see?");
            string userName = Console.ReadLine().Trim();

            ViewUserTimeStamps(userName);

        }

        private void ViewTimeStampsBetweenTimeSpans(DateTime from, DateTime to)
        {
            List<TimeStamp> timeStamps = timeStampPersister.GetObjects()
            .Where(ts => ts.CheckIn >= from && ts.CheckOut <= to).Select(ts => ts).ToList();

            LoopThroughList(timeStamps);

        }

        private void ViewTimeStampsBetweenTimeSpans()
        {
            Console.WriteLine("Format: HH:mm:ss YYYY-dd-MM");
            Console.WriteLine("From?");
            string fromDate = Console.ReadLine();
            DateTime from = DateTime.Parse(fromDate);
            Console.WriteLine("To?");
            string toDate = Console.ReadLine();
            DateTime to = DateTime.Parse(toDate);

            ViewTimeStampsBetweenTimeSpans(from, to);
        }

        private void ViewAllTimeSpans()
        {
            List<TimeStamp> timeStamps = timeStampPersister.GetObjects();
            LoopThroughList(timeStamps);
        }
        private void ViewTimeSpans()
        {
            while (true)
            {
                Console.WriteLine("1. My timestamps\n 2. All timestamps\n3. Timestamps between dates\n4. Other users timespans\n5. Return");
                int choice = GetNumberInput();

                switch (choice)
                {
                    case 1: ViewUserTimeStamps(LOGGED_IN_USER.UserName); break;
                    case 2: ViewAllTimeSpans(); break;
                    case 3: ViewTimeStampsBetweenTimeSpans(); break;
                    case 4: ViewUserTimeStamps(); break;
                    case 5: return;
                    default: Console.WriteLine("Invalid input"); break;
                }
            }
           

        }



        private TimeStamp CreateTimeStamp()
        {
            ChangeTextColor(ConsoleColor.Yellow);
            Console.WriteLine("Creating timestamp");
            TimeStamp timeStamp = new TimeStamp(DateTime.Now);
            LOGGED_IN_USER.AddTimeStamp(timeStamp);

            return timeStamp;
        }

        private void CheckOut()
        {
            TimeStamp timeStamp = LOGGED_IN_USER.TimeStamps[^1];
            timeStamp.CheckOut = DateTime.Now;
            LOGGED_IN = false;
            LOGGED_IN_USER = null;

            timeStampPersister.SaveObject(timeStamp);

            ChangeTextColor(ConsoleColor.Yellow);
            Console.WriteLine("Checking out.");
            Console.WriteLine("Logging out.");

        }
        private void ViewAllUsers()
        {
            List<User> users = userPersister.GetObjects();
            LoopThroughList(users);
        }
        public User CreateUser()
        {
            string userName = GetInput("Username");
            string password = GetInput("Password");
            string password2 = GetInput("Confirm password");

            if (password.Equals(password2))
            {
                User user = new User(userName, password);
                userPersister.SaveObject(user);
                return user;
            }

            throw new Exception();
        }
        public void TryToLogin()
        {
            string userName = GetInput("Username");
            string password = GetInput("Password");

            try
            {
                User user = Login.TryLogin(userName, password);
                LOGGED_IN = true;
                LOGGED_IN_USER = user;
                CreateTimeStamp();

            }
            catch (Exception)
            {
                LOGGED_IN = false;
                ChangeTextColor(ConsoleColor.Red);
                Console.WriteLine("Login failed.");
            }

            Console.Clear();

        }
        public string GetInput(string command, ConsoleColor color = ConsoleColor.Green)
        {
            ChangeTextColor(color);
            Console.WriteLine($"{command}:");
            return Console.ReadLine();
        }

        public int GetNumberInput()
        {
            return Int32.Parse(Console.ReadLine().Trim());
        }

        public void ChangeTextColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
