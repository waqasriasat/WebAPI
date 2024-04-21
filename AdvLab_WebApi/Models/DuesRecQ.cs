namespace AdvLab_WebApi.Models
{
    public class DuesRecQ
    {
        public int id { get; set; }
        public Nullable<int> LabNo { get; set; }
        public Nullable<double> AmountRec { get; set; }
        public Nullable<System.DateTime> ActionDate { get; set; }
        public Nullable<int> UId { get; set; }
        public string Uname { get; set; }
        public string Idloc { get; set; }
    }
}
