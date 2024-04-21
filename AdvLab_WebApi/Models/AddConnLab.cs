namespace AdvLab_WebApi.Models
{
    public class AddConnLab
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public string LocActive { get; set; }
        public Nullable<int> LocID { get; set; }
        public string LocCate { get; set; }
        public string Location { get; set; }
        public Nullable<int> UId { get; set; }
        public string? Idloc { get; set; }
        public string? CompMac { get; set; }
    }
}
