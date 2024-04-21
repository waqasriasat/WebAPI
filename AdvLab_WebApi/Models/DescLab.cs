using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvLab_WebApi.Models
{
    public class DescLab
    {
        public DescLab()
        {
        }
        [Key]
        public int ID { get; set; }
        [ForeignKey("DescCashier")]
        public int LabNo { get; set; }
        public virtual DescCashier? DescCashier { get; private set; }
        public string? BarcodeNo { get; set; }
        public int DescID { get; set; }
        public System.DateTime? RepDate { get; set; }
        public int Charges { get; set; }
        public string? DStatus { get; set; }
        public System.DateTime? StatusDate { get; set; }

    }
}
