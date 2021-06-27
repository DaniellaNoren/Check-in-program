using CheckInProgram.Persists;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckInProgram
{
    public class User
    {
        private static int shared_id;

        private string userName;
        public string UserName { get { return userName; } set { userName = value; } }  //TODO: Regex. Check if Username is available?

        private string password;
        public string Password { get { return password; } set { password = value; } } //TODO: Don't store in string. Regex.

        private string id;
        public string Id { get { return id; } set { id = value; } }

        public List<TimeStamp> TimeStamps { get; set; }

        public User(string userName, string password)
        {
            this.Password = password;
            this.UserName = userName;
            this.Id = (++shared_id).ToString();
        }

        public override string ToString()
        {
            string userInfo = $"Username: {UserName}, Id: {Id}, ";

            string timeStamps = "";

            if (TimeStamps != null)
            {
                foreach (TimeStamp timeStamp in TimeStamps)
                {
                    timeStamps += timeStamps + " | ";
                }

            }

            return userInfo + timeStamps;

        }

        public void AddTimeStamp(TimeStamp timeStamp)
        {
            if (TimeStamps == null)
                TimeStamps = new List<TimeStamp>();

            timeStamp.User = this;
            TimeStamps.Add(timeStamp);
        }

        public void DeleteTimeStamp(TimeStamp timeStamp)
        {
            timeStamp.User = null;
            TimeStamps.Remove(timeStamp);
        }

    }

    public class Login
    {
        private static readonly IPersister<User> Persister = new FileUserPersister();
        public static User TryLogin(string username, string password)
        {
            User user = LookupUser(username);
            bool successfullLogin = ComparePasswords(user.Password, password);

            if (successfullLogin)
            {
                return user;
            }
            else
            {
                throw new Exception(); //TODO make better exception
            }
        }

        public static bool ComparePasswords(string password, string sentInPassword)
        {
            return password.Equals(sentInPassword);
        }
        public static User LookupUser(string username)
        {
            return Persister.GetObject($"\"UserName\":\"{username}\"");
        }
    }
}
