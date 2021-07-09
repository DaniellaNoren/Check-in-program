using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckInProgram.Persists
{
    [JsonConverter(typeof(TimeStamp))]
    public class TimeStampConverter : JsonConverter<TimeStamp>
    {
        public override TimeStamp ReadJson(JsonReader reader, Type objectType, TimeStamp existingValue, bool hasExistingValue, JsonSerializer serializer)
        {

            TimeStamp timeStamp = new TimeStamp();

            while (reader.Read())
            {
                if (null == reader.Value)
                    break;

                if (reader.Value.Equals("Check-in"))
                    timeStamp.CheckIn = (DateTime)reader.ReadAsDateTime();
                else if (reader.Value.Equals("Check-out"))
                    timeStamp.CheckOut = (DateTime)reader.ReadAsDateTime();
                else if (reader.Value.Equals("User"))
                {
                    reader.Read();
                    timeStamp.User = serializer.Deserialize<User>(reader);

                }
                else if (reader.Value.Equals("Id"))
                {
                    timeStamp.guid = Guid.Parse(reader.ReadAsString());
                }
            }

            return timeStamp;

        }

        public override void WriteJson(JsonWriter writer, TimeStamp value, JsonSerializer serializer)
        {
            writer.WriteToken(JsonToken.StartObject);
            writer.WriteToken(JsonToken.PropertyName, "Check-in");
            writer.WriteToken(JsonToken.String, value.CheckInString);
            writer.WriteToken(JsonToken.PropertyName, "Check-out");
            writer.WriteToken(JsonToken.String, value.CheckOutString);
            writer.WriteToken(JsonToken.PropertyName, "User");
            writer.WriteToken(JsonToken.StartObject);
            writer.WriteToken(JsonToken.PropertyName, "UserName");
            writer.WriteToken(JsonToken.String, value.User.UserName);
            writer.WriteToken(JsonToken.EndObject);
            writer.WriteToken(JsonToken.PropertyName, "Id");
            writer.WriteToken(JsonToken.String, value.guid);
            writer.WriteToken(JsonToken.EndObject);

        }
    }
}
