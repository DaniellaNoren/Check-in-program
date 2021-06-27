using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckInProgram
{
    public class ObjectParser
    {
        public static string GetJsonFromObject(object objToSave)
        {
            return JsonConvert.SerializeObject(objToSave);
        }

        public static object GetObjectFromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject(jsonString);
        }

        public static string[] GetJsonsFromObjects(object[] objectsToSave)
        {
            int arrLength = objectsToSave.Length;
            string[] jsonStrings = new string[arrLength];

            for (int i = 0; i < arrLength; i++)
            {
                jsonStrings[i] = GetJsonFromObject(objectsToSave[i]);
            }

            return jsonStrings;
        }  
        
        public static object[] GetObjectsFromJsons(string[] jsonStrings)
        {
            int arrLength = jsonStrings.Length;
            object[] objects = new object[arrLength];

            for (int i = 0; i < arrLength; i++)
            {
                objects[i] = GetObjectFromJson(jsonStrings[i]);
            }

            return objects;
        }

    }
}
