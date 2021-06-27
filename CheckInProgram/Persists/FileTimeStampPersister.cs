using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckInProgram.Persists
{
    public class FileTimeStampPersister : IPersister<TimeStamp>
    {
        private readonly string FILE_NAME = @"./Timestamps.txt";
        
        public TimeStamp GetObject(string identifier)
        {
            string jsonString = FileSaver.GetLineFromFile(identifier, FILE_NAME);

            if (string.IsNullOrEmpty(jsonString))
                return new TimeStamp(DateTime.Now, new User("", ""));

            return (TimeStamp) ObjectParser.GetObjectFromJson<TimeStamp>(jsonString);
        }

        public List<TimeStamp> GetObjects()
        {
            string[] jsonString = FileSaver.GetAllLinesFromFile(FILE_NAME);

            List<TimeStamp> timeStamps = new List<TimeStamp>(ObjectParser.GetObjectsFromJsons<TimeStamp>(jsonString));

            return timeStamps;
        }

        public void SaveObject(TimeStamp timeStamp)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            
            string jsonString = ObjectParser.GetJsonFromObject(timeStamp, settings);
            FileSaver.SaveText(jsonString, FILE_NAME);
        }

        public void DeleteObject(string identifier)
        {
            FileSaver.RemoveLineFromFile(identifier, FILE_NAME);
        }
        public void UpdateObject(TimeStamp timeStamp, string identifier)
        {
            throw new NotImplementedException();
        }
    }
}
