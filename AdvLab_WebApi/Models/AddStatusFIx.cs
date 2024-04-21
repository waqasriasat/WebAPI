using System.ComponentModel.DataAnnotations;

namespace AdvLab_WebApi.Models
{
    public class AddStatusFIx
    {
        [Key]
        public int SOR { get; set; }
        public string DStatus { get; set; }
    }
}
