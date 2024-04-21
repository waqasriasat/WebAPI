namespace AdvLab_WebApi.Models
{
    public class AccessRight
    {
        public int ID { get; set; }
        public Nullable<int> SOR { get; set; }
        public Nullable<int> EmpID { get; set; }
        public string Assigning { get; set; }
    }
}
