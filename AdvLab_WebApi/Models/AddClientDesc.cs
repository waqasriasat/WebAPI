using System.ComponentModel.DataAnnotations;

namespace AdvLab_WebApi.Models
{
    public class AddClientDesc
    {
        [Key]
        public int ID { get; set; }
        public int CID { get; set; }
        public int DescID { get; set; }
        public float Price { get; set; }
        public float ActualPrice { get; set; }
        public float PDisc { get; set; }
        public float TDisc { get; set; }
    }
}
