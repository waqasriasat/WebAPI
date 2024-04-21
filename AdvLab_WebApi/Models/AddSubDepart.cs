namespace AdvLab_WebApi.Models
{
    public class AddSubDepart
    {
        public int ID { get; set; }
        public Nullable<int> SID { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public string? Location { get; set; }
        public string Depart { get; set; }
        public string SubDepart { get; set; }
        public Nullable<int> UId { get; set; }
        public string? Idloc { get; set; }
        public string? CompMac { get; set; }
    }
}
