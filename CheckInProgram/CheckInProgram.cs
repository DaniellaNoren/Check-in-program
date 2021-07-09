using CheckInProgram.Cryptography;
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
        private bool IS_ADMIN;

        private static readonly IPersister<TimeStamp> timeStampPersister = new FileTimeStampPersister();
        private static IPersister<User> userPersister = new FileUserPersister();


        static void Main(string[] args)
        {
            KeyHolder.GenerateKeyIVAndEntropy();
            KeyHolder.ReadKeyAndIv();

            CheckInProgram program = new CheckInProgram();

            program.MenuLoop();
        }

        public void MenuLoop()
        {
            while (CONTINUE_PROGRAM)
            {
                ChangeTextColor(ConsoleColor.Blue);

                if (!LOGGED_IN)
                    Console.WriteLine("1. Login\n2. Create user\n3. Create default admin\n4. Exit");
                else
                    Console.WriteLine("1. Print something funny\n2. View all users\n3. View timestamps\n4. Log out");

                int choice = GetNumberInput("");
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
                    case 2:
                        {
                            User user = CreateUser();
                            LOGGED_IN_USER = user;
                            LOGGED_IN = true;
                        }
                        break;
                    case 3: CreateDefaultAdmin(); break;
                    case 4: Console.WriteLine("Bye!"); CONTINUE_PROGRAM = false; break;
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

        private void CreateDefaultAdmin()
        {
            if(GetUser("admin") == null)
            {
                User admin = new User("admin", "admin", new List<UserRole>() { UserRole.User, UserRole.Admin });
                userPersister.SaveObject(admin);
                Console.WriteLine("Admin created");
                Console.WriteLine(admin.ToString());
            }
            else
            {
                Console.WriteLine("Admin already exists. Username: 'admin', Password: 'admin'");
            }
           
        }

        private void LoopThroughList<T>(List<T> list, bool showNrs = false)
        {
            if (showNrs)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {list[i]}");
                }
            }
            else
            {
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
            }

        }

        private void GetTimeStamps(string query)
        {
            List<TimeStamp> timeStamps = timeStampPersister.GetObjects(query);
            LoopThroughList(timeStamps);

        }
        private void ViewUserTimeStamps()
        {
            string userName = GetInput("Which user's timestamps do you want to see?");

            GetTimeStamps($"\"User\":{{\"UserName\":\"{userName}\"");

        }

        private void ViewTimeStampsOfDate()
        {

            string date = GetInput("What day's timestamps do you want to see? (YYYY-mm-dd)", pattern: "^20[2-9][0-9]-[0-1][1-9]-[0-3][0-9]$"); 

            ViewTimeStampsBetweenTimeSpans(DateTime.Parse(date), DateTime.Parse(date).AddHours(23).AddMinutes(59));
        }

        private void ViewTimeStampsBetweenTimeSpans(DateTime from, DateTime to)
        {
            List<TimeStamp> timeStamps = timeStampPersister.GetObjects()
            .Where(ts => ts.CheckIn >= from && ts.CheckOut <= to).Select(ts => ts).ToList();

            LoopThroughList(timeStamps);

        }

        private void ViewTimeStampsBetweenTimeSpans()
        {
            string datePattern = "^[0-5][0-5]:[0-5][0-5]:[0-5][0-5] 20[2-9][0-9]-[0-1][1-9]-[0-3][0-9]$";
            Console.WriteLine("Format: HH:mm:ss YYYY-dd-MM");
            string fromDate = GetInput("From", pattern: datePattern);
            DateTime from = DateTime.Parse(fromDate);
            string toDate = GetInput("To", pattern: datePattern);
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
                Console.WriteLine("1. My timestamps\n2. All timestamps\n3. Timestamps on day\n4. Timestamps between two dates\n5. Other users timespans\n6. Go back");

                int choice = GetNumberInput("");

                switch (choice)
                {
                    case 1: GetTimeStamps($"\"User\":{{\"UserName\":\"{LOGGED_IN_USER.UserName}\""); break;
                    case 2: ViewAllTimeSpans(); break;
                    case 3: ViewTimeStampsOfDate(); break;
                    case 4: ViewTimeStampsBetweenTimeSpans(); break;
                    case 5: ViewUserTimeStamps(); break;
                    case 6: return;
                    default: Console.WriteLine("Invalid input"); break;

                }
            }


        }

        private TimeStamp CreateTimeStamp()
        {
            ChangeTextColor(ConsoleColor.Yellow);
            Console.WriteLine("Creating timestamp");
            TimeStamp timeStamp = new TimeStamp(DateTime.Now, LOGGED_IN_USER);
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

            if (IS_ADMIN)
                EditUsers();
        }

        private void EditUsers()
        {
            Console.WriteLine("1. Edit user\n2. Go back");
            int choice = GetNumberInput("");

            switch (choice)
            {
                case 1: EditUser(); break;
                case 2: return;
            }
        }

        private string ChooseUser()
        {
            string id = GetInput("ID of user");

            return id;
        }

        private void DeleteUser(User user)
        {
            string answer = GetInput("Are you sure? Y/N");

            if (answer.Equals("Y"))
            {
                userPersister.DeleteObject($"\"Id\":\"{user.Id}\"");

            }
            else
                Console.WriteLine("Returning");
        }

        private void EditUser()
        {
            string id = ChooseUser();

            User user = userPersister.GetObject($"\"Id\":\"{id}\"");

            if (user == null)
            {
                Console.WriteLine("User does not exist.");
                return;
            }
            else
            {

                bool keepEditing = true;

                while (keepEditing)
                {
                    Console.WriteLine($"What do you want to do with {user.UserName}?");
                    Console.WriteLine("1. Edit username\n2. Edit password\n3. Edit roles\n4. Delete User\n5. Save\n6. Return without saving");
                    int choice = GetNumberInput("");

                    switch (choice)
                    {
                        case 1: user.UserName = GetInput("New username"); break;
                        case 2: user.Password = GetInput("New password"); break;
                        case 3:
                            {
                                if (!user.UserRoles.Contains(UserRole.Admin))
                                    Console.WriteLine("1. Make Admin");
                                else
                                    Console.WriteLine("1. Remove Admin-role");

                                Console.WriteLine("2. Return");
                                int choice2 = GetNumberInput("");

                                switch (choice2)
                                {
                                    case 1:
                                        {
                                            if (!user.UserRoles.Contains(UserRole.Admin))
                                            {
                                                user.UserRoles.Add(UserRole.Admin);
                                                Console.WriteLine("User is now admin");
                                            }
                                            else
                                            {
                                                user.UserRoles.Remove(UserRole.Admin);
                                                Console.WriteLine("User is no longer admin");
                                            }
                                        }
                                        break;
                                    case 2: break;
                                    default: break;
                                }
                                break;

                            }
                        case 4: DeleteUser(user); return;
                        case 5: userPersister.UpdateObject(user, $"\"Id\":\"{user.Id}\""); Console.WriteLine("User saved"); keepEditing = false; break;
                        case 6: keepEditing = false; break;
                        default: break;
                    }
                }

            }
        }

        public User GetUser(string userName)
        {
            return userPersister.GetObject($"\"UserName\":\"{userName}\"");
        }
        public User CreateUser()
        {
            User createdUser;

            string userName;
            while (true)
            {
                userName = GetInput("Username");
                if (GetUser(userName) != null)
                {
                    Console.WriteLine("Username already exists, try again");
                    continue;
                }
                break;

            }

            while (true)
            {
                string password = GetInput("Password");
                string password2 = GetInput("Confirm password");

                if (password.Equals(password2))
                {
                    createdUser = new User(userName, password);
                    userPersister.SaveObject(createdUser);
                    break;
                }

                Console.WriteLine("Passwords do not match, try again.");
            }

            Console.Clear();

            return createdUser;

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
                IS_ADMIN = user.UserRoles.Contains(UserRole.Admin);
                CreateTimeStamp();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                LOGGED_IN = false;
                ChangeTextColor(ConsoleColor.Red);
                Console.WriteLine("Login failed.");
            }

            Console.Clear();

        }

        public int GetNumberInput(string command, ConsoleColor color = ConsoleColor.Green)
        {
            ChangeTextColor(color);
            if (!string.IsNullOrEmpty(command))
                Console.WriteLine($"{command}:");

            int input;

            while (true)
            {
                try
                {
                    input = InputHandler.GetInt();
                    break;
                }
                catch (InvalidInputException)
                {
                    Console.WriteLine("Invalid input, try again.");
                    Console.WriteLine(command);
                }

            }

            return input;
        }

        public string GetInput(string command, ConsoleColor color = ConsoleColor.Green, string pattern = "")
        {
            ChangeTextColor(color);
            string input;

            while (true)
            {
                try
                {
                    Console.WriteLine($"{command}:");

                    input = InputHandler.GetString(pattern);
                    break;
                }
                catch (InvalidInputException)
                {
                    Console.WriteLine("Invalid input, try again.");
                    Console.WriteLine(command);
                }

            }

            return input;
        }


        public void ChangeTextColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }
    }
}
