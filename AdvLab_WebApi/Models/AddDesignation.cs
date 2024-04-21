namespace AdvLab_WebApi.Models
{
    public class AddDesignation
    {
        public int ID { get; set; }
        public Nullable<int> SID { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public string Designation { get; set; }
        public Nullable<int> UId { get; set; }
        public string? Idloc { get; set; }
        public string? CompMac { get; set; }
    }
}
