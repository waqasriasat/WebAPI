using System.ComponentModel.DataAnnotations;

namespace AdvLab_WebApi.Models
{
    public class AddDescription
    {
        [Key]
        public int ID { get; set; }
        public int DescID { get; set; }
        public string TestCode { get; set; }
        public string DescName { get; set; }
        public string Lock { get; set; }
        public string Cate { get; set; }
        public string Location { get; set; }
        public string Depart { get; set; }
        public string SubDepart { get; set; }
        public string PerformLab { get; set; }
        public string Category { get; set; }
        public string SampleSended { get; set; }
        public int Charges { get; set; }
        public string TRDTypr { get; set; }
        public string TProcedure { get; set; }
        public int DValue { get; set; }
    }
}
