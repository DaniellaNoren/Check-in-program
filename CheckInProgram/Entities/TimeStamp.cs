using System;

namespace CheckInProgram
{ 
    public class TimeStamp
    {
        private static readonly string dateTimeFormatString = "HH:mm:ss yyyy-MM-dd";

        private User user;
        public DateTime CheckIn { get; set; }
        public string CheckInString { get { return CheckIn.ToString(dateTimeFormatString); } }
        public DateTime CheckOut { get; set; }
        public string CheckOutString { get { return CheckOut.ToString(dateTimeFormatString); } }
        public User User { get { return user; } set { user = value; } }

        public Guid guid { get; set; }
       
        public TimeStamp()
        {
            this.guid = Guid.NewGuid();
        }
        public TimeStamp(DateTime checkIn, User user) : this()
        {
            this.User = user;
            this.CheckIn = checkIn;
        } 

        public TimeStamp(DateTime checkIn, DateTime checkOut, User user) : this(checkIn, user)
        {
            this.CheckOut = checkOut;
        }
        public override string ToString()
        {
            return $"Check-in: {CheckInString}, Check-out: {CheckOutString}, User: {User.UserName}, Id: {guid}";
        }       

    }
   

}
