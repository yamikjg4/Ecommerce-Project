using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class ChangePasswordModel
    {
        [Required, DataType(DataType.Password), Display(Name = "CurrentPassword")]
        public string Currentpassword { get; set; }
        [Required, DataType(DataType.Password), Display(Name = "NewPassword")]
        public string Newpassword { get; set; }
        [Compare("Newpassword", ErrorMessage = "Password Not Match")]
        public string ConfirmPassword { get; set; }
    }
}
