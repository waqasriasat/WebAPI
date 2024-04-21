using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvLab_WebApi.Models
{
    public class DescCashier
    {
        public DescCashier()
        {
        }
        [Key]
        public int LabNo { get; set; }
        [ForeignKey("PatReg")]
        public int MRNO { get; set; }
        public virtual PatReg? PatReg { get; private set; }
        public string? CpName { get; set; }
        public System.DateTime RegDate { get; set; }
        public string? CpNo { get; set; }
        public int ClientID { get; set; }
        public string? ConsName { get; set; }
        public string? ClientNo { get; set; }
        public string? Comments { get; set; }
        public double GrossA { get; set; }
        public double DiscPer { get; set; }
        public double Discount { get; set; }
        public double TotalA { get; set; }
        public double PaidA { get; set; }
        public double BlanceA { get; set; }
        public string? PaymentMode { get; set; }
        public string? CreditCardNo { get; set; }
        public double? TaxN { get; set; }
        public int UId { get; set; }
        public string? Idloc { get; set; }
        public DateTime? RbalanceDate { get; set; }
        public double? RbalanceA { get; set; }
        public int RUId1 { get; set; }
        public string? RIdloc1 { get; set; }
        public double? CurrentB { get; set; }
        public int BillNo { get; set; }
        public string? F_VNo { get; set; }
        public string? CStatus { get; set; }
        public string? RV { get; set; }
        public string? SV { get; set; }
        public string? RV1 { get; set; }
        public string? Pwd { get; set; }
        public string? InvoiceSMS { get; set; }
        public string? DortocSMS { get; set; }
        public virtual List<DescLab> DescLabs { get; set; } = new List<DescLab>();
    }
}
