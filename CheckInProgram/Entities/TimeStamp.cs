using CheckInProgram.Persists;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace CheckInProgram
{
    
    public class TimeStamp
    {
        private static string DateTimeFormatString = "HH:mm:ss yyyy-MM-dd";

        [JsonIgnore]
        private User user;
        public DateTime CheckIn { get; set; }
        [JsonIgnore]
        public string CheckInString { get { return CheckIn.ToString(DateTimeFormatString); } }
        
        public DateTime CheckOut { get; set; }
        [JsonIgnore]
        public string CheckOutString { get { return CheckOut.ToString(DateTimeFormatString); } }
       
       
        public User User { get { return user; } set { user = value; } }
        [JsonIgnore]
        public string UserName { get { return user.UserName; } }
      
        public TimeStamp()
        {

        }
        public TimeStamp(DateTime checkIn)
        {
            this.CheckIn = checkIn;
        }
        public TimeStamp(DateTime checkIn, User user) : this(checkIn)
        {
            this.User = user;
        } 

        public TimeStamp(DateTime checkIn, DateTime checkOut, User user) : this(checkIn, user)
        {
            this.CheckOut = checkOut;
        }
        public override string ToString()
        {
            return $"Check-in: {CheckInString}, Check-out: {CheckOutString}, User: {User.UserName}";
        }       

    }
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

                if(reader.Value.Equals("Check-in"))
                    timeStamp.CheckIn = (DateTime)reader.ReadAsDateTime();
                else if(reader.Value.Equals("Check-out"))
                    timeStamp.CheckOut = (DateTime)reader.ReadAsDateTime();
                else if (reader.Value.Equals("User"))
                {
                    reader.Read();
                    timeStamp.User = serializer.Deserialize<User>(reader);

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
            writer.WriteToken(JsonToken.EndObject);

        }
    }

}
