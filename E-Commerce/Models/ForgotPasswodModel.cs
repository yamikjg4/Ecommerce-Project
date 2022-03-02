using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class ForgotPasswodModel
    {
        [Required, EmailAddress]
        [Display(Name = "Register EmailAddress")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }

    }
}
