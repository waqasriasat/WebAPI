using System.ComponentModel.DataAnnotations;

namespace AdvLab_WebApi.Models.Temp
{
    public class ChangePassword
    {
        public int EmpId { get; set; }

        public string CurrentPassword { get; set; }

        public string? NewPassword { get; set; }

        public string ConfirmNewPassword { get; set; }
    }
}
