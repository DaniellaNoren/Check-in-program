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

            return (TimeStamp)ObjectParser.GetObjectFromJson<TimeStamp>(jsonString.Trim(), new TimeStampConverter());
        }

        public List<TimeStamp> GetObjects()
        {
            string[] jsonString = FileSaver.GetAllLinesFromFile(FILE_NAME);

            List<TimeStamp> timeStamps = new List<TimeStamp>(ObjectParser.GetObjectsFromJsons<TimeStamp>(jsonString, new TimeStampConverter()));

            return timeStamps;
        }
        public List<TimeStamp> GetObjects(string identifier)
        {
            string[] jsonString = FileSaver.GetLinesFromFile(identifier, FILE_NAME);

            List<TimeStamp> timeStamps = new List<TimeStamp>(ObjectParser.GetObjectsFromJsons<TimeStamp>(jsonString, new TimeStampConverter()));

            return timeStamps;
        }

        public void SaveObject(TimeStamp timeStamp)
        {
            string jsonString = ObjectParser.GetJsonFromObject(timeStamp, new TimeStampConverter());
            FileSaver.SaveText(jsonString, FILE_NAME);
        }

        public void DeleteObject(string identifier)
        {
            FileSaver.RemoveLineFromFile(identifier, FILE_NAME);
        }
        public void UpdateObject(TimeStamp timeStamp, string identifier)
        {
            string jsonString = ObjectParser.GetJsonFromObject(timeStamp);
            FileSaver.ReplaceLine(identifier, jsonString, FILE_NAME);
        }
    }
}
