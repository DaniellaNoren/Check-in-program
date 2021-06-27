using Newtonsoft.Json;
using System;

namespace CheckInProgram
{
    public class ObjectParser  //TODO: Handle exceptions
    {
        public static string GetJsonFromObject(object objToSave)
        {
            return JsonConvert.SerializeObject(objToSave);
        }

        public static object GetObjectFromJson<T>(string jsonString)
        {
            T t = JsonConvert.DeserializeObject<T>(jsonString.Trim());
            return t;
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

        public static T[] GetObjectsFromJsons<T>(string[] jsonStrings)
        {
            int arrLength = jsonStrings.Length;
            T[] objects = new T[arrLength];

            for (int i = 0; i < arrLength; i++)
            {
                objects[i] = (T)GetObjectFromJson<T>(jsonStrings[i]);
            }

            return objects;
        }

    }
}
