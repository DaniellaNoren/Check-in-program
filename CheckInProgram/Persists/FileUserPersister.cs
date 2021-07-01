using System;
using System.Collections.Generic;
using System.Text;

namespace CheckInProgram.Persists
{
    public class FileUserPersister : IPersister<User>
    {

        private readonly string FILE_NAME = @"./Users.txt";
        public User GetObject(string identifier)
        {
            string jsonString = FileSaver.GetLineFromFile(identifier, FILE_NAME);

            if (string.IsNullOrEmpty(jsonString))
                return new User("", "");

            return (User) ObjectParser.GetObjectFromJson<User>(jsonString);
        }

        public List<User> GetObjects()
        {
            string[] jsonString = FileSaver.GetAllLinesFromFile(FILE_NAME);

            List<User> users = new List<User>(ObjectParser.GetObjectsFromJsons<User>(jsonString));

            return users;
        }

        public List<User> GetObjects(string identifier)
        {
            string[] jsonString = FileSaver.GetLinesFromFile(identifier, FILE_NAME);

            List<User> users = new List<User>(ObjectParser.GetObjectsFromJsons<User>(jsonString));

            return users;
        }

        public void SaveObject(User user)
        {
            string jsonString = ObjectParser.GetJsonFromObject(user);
            FileSaver.SaveText(jsonString, FILE_NAME);
        }
        public void DeleteObject(string identifier)
        {
            FileSaver.RemoveLineFromFile(identifier, FILE_NAME);
        }

        public void UpdateObject(User user, string identifier)
        {
            throw new NotImplementedException();
        }
    }
}
