using Newtonsoft.Json;
using System;

namespace CheckInProgram
{
    
    public class TimeStamp
    {
        private static string DateTimeFormatString = "HH:mm:ss yyyy-MM-dd";

        [JsonIgnore]
        private User user;
        public DateTime CheckIn { get; set; }
        [JsonProperty(PropertyName = "Check-in")]
        public string CheckInString { get { return CheckIn.ToString(DateTimeFormatString); } }
        [JsonIgnore]
        public DateTime CheckOut { get; set; }
        [JsonProperty(PropertyName = "Check-out")]
        public string CheckOutString { get { return CheckOut.ToString(DateTimeFormatString); } }
        [JsonIgnore]
        public User User { get { return user; } set { user = value; } }
        [JsonProperty(PropertyName = "User")]
        public string UserName { get { return user.UserName; } }
       
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
            return $"Check-in: {CheckInString}, Check-out: {CheckOutString}, User: {UserName}";
        }



    }
}
