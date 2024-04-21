namespace AdvLab_WebApi.Models
{
    public class AddPlacement
    {
        public int ID { get; set; }
        public Nullable<int> SID { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public string PlaceCode { get; set; }
        public string PlaceName { get; set; }
        public Nullable<int> UId { get; set; }
        public string? Idloc { get; set; }
        public string? CompMac { get; set; }
    }
}
