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
        public string UserName {  get { return userName; } set { userName = value; } }  //TODO: Regex. Check if Username is available?

        private string password;
        public string Password { get { return password; } set { password = value; } } //TODO: Don't store in string. Regex.

        private string id;
        public string Id { get { return id;  } set { id = value; } }

        public User(string userName, string password)
        {
            this.Password = password;
            this.UserName = userName;
            this.Id = (++shared_id).ToString();
        }

    }

    public class Login
    {
        private static readonly IPersister<User> Persister = new FileUserPersister();
        public static bool TryLogin(string username, string password)
        {
            User user = LookupUser(username);
            return ComparePasswords(user.Password, password);
        }

        public static bool ComparePasswords(string password, string sentInPassword)
        {
            return password.Equals(sentInPassword);
        }
        public static User LookupUser(string username)
        {
            try
            {
               return Persister.GetObject($"UserName\":\"{username}\"");
            }catch(Exception)
            {
                return new User("", "");
            }
        }
    }
}
