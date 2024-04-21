

namespace AdvLab_WebApi.Models
{
    public class ReceptionViewModels
    {
    }
    public class CreateViewModel
    {

        public PatReg PatReg { get; set; }
        public DescCashier DescCashier { get; set; }
    }

    public class PatRegViewModel
    {
        public PatReg PatReg { get; set; }
        public int LabNo { get; set; }
    }
}
