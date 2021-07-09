using CheckInProgram.Persists;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckInProgram
{
    public class User
    {
        private string userName;
        public string UserName { get { return userName; } set { userName = value; } }  //TODO: Regex. Check if Username is available?

        private string password;
        public string Password { get { return password; } set { password = value; } } //TODO: Don't store in string. Regex.

        private Guid id;
        public Guid Id { get { return id; } set { id = value; } }

        [JsonIgnore]
        public List<TimeStamp> TimeStamps { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, ItemConverterType = typeof(StringEnumConverter))]
        public List<UserRole> UserRoles { get; set; }

        public User()
        {

        }

        public User(string userName, string password, List<UserRole> userRoles)
        {
            this.Password = password;
            this.UserName = userName;
            this.Id = Guid.NewGuid();
            this.UserRoles = userRoles;
        }

        public User(string userName, string password) : this(userName, password, new List<UserRole>() { UserRole.User })
        {

        }

        public override string ToString()
        {
            string userInfo = $"Username: {UserName}, Id: {Id}, ";

            string timeStamps = "TimeStamps: ";

            if (TimeStamps != null)
            {
                foreach (TimeStamp timeStamp in TimeStamps)
                {
                    timeStamps += timeStamps + " | ";
                }

            }
            else
            {
                timeStamps += " [] ";
            }

            string userRoles = ", Roles: ";

            foreach (UserRole role in UserRoles)
            {
                userRoles += $"{role}, ";
            }

            return userInfo + timeStamps + userRoles;

        }

        public void AddRole(UserRole role)
        {
            this.UserRoles.Add(role);
        }

        public void RemoveRole(UserRole role)
        {
            this.UserRoles.Remove(role);
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
    public enum UserRole
    {
        User, Admin
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
                throw new FailedLoginException("Username or password is wrong.");
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

    public class FailedLoginException : Exception
    {
        public FailedLoginException(string msg) : base(msg) { }
    }
}
