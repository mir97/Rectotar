using System.ComponentModel.DataAnnotations;

namespace Rectotarat.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
