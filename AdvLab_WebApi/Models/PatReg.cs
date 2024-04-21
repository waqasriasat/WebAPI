using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdvLab_WebApi.Models
{
    public class PatReg
    {
        [Key]
        public int MRNO { get; set; }
        public System.DateTime RegDate { get; set; }
        public System.DateTime LastUpdate { get; set; }
        public string Initial { get; set; }
        public string FirstName { get; set; }
        public string Relation { get; set; }
        public string? RelName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string AgeType { get; set; }
        public System.DateOnly? DBO { get; set; }
        public string ContNo { get; set; }
        public string? Email { get; set; }
        public int UId { get; set; }
        public string? Idloc { get; set; }
        public string? PhotoUrl { get; set; }

    }
}
