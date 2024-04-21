using System.ComponentModel.DataAnnotations;

namespace AdvLab_WebApi.Models
{
    public class PatReg_Shortkey
    {
        [Key]
        public int Sno { get; set; }
        public string Initial { get; set; }
        public string Relation { get; set; }
        public string Years { get; set; }
        public string Gender { get; set; }
    }
}
