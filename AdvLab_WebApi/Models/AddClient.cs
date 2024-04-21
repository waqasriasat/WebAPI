using System.ComponentModel.DataAnnotations;

namespace AdvLab_WebApi.Models
{
    public class AddClient
    {
        [Key]
        public int ID { get; set; }
        public System.DateTime RegDate { get; set; }
        public int CID { get; set; }
        public string CName { get; set; }
        public string ComName { get; set; }
        public string ClientActive { get; set; }
        public string PerA { get; set; }
        public string Pwd { get; set; }
        public string DescType { get; set; }
        public string PriceChangabletrue { get; set; }
        public string Location { get; set; }
        public string CLocation { get; set; }
        public string CCont { get; set; }
        public string CEmail { get; set; }
        public string Address { get; set; }
        public double Inc_Routine { get; set; }
        public double Inc_Special { get; set; }
        public double Inc_NoDisc { get; set; }
        public double Inc_Xray { get; set; }
        public double Inc_Ultra { get; set; }
        public double Inc_Ctscan { get; set; }
        public double Inc_Mri { get; set; }
        public double Inc_Echo { get; set; }
        public double Inc_Ecg { get; set; }
        public double Inc_Cdopler { get; set; }
        public int UId { get; set; }
        public int BDO { get; set; }
        public string BusinessType { get; set; }
        public string ClientInstraction { get; set; }
        public string PAWC { get; set; }
        public double Dsc_Routine { get; set; }
        public double Dsc_Special { get; set; }
        public double Dsc_NoDisc { get; set; }
        public double Dsc_Xray { get; set; }
        public double Dsc_Ultra { get; set; }
        public double Dsc_Ctscan { get; set; }
        public double Dsc_Mri { get; set; }
        public double Dsc_Echo { get; set; }
        public double Dsc_Ecg { get; set; }
        public double Dsc_Cdopler { get; set; }
    }
}
