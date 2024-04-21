namespace AdvLab_WebApi.Models
{
    public class LoginLog
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> logDate { get; set; }
        public string CompName { get; set; }
        public string CompUser { get; set; }
        public string IP { get; set; }
        public string MacAddress { get; set; }
        public string LoginStatus { get; set; }
        public Nullable<int> UserID { get; set; }
        public string UserName { get; set; }
    }
}
