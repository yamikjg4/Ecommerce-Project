using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class singupmodel
    {
        [Required]
        /*[RegularExpression("[a-zA-Z]+", ErrorMessage = "Alphabet is Not allowed")]*/
        [Display(Name = "User Name")]
        public string Name { get; set; }
        [Required, EmailAddress]
        [DataType(DataType.EmailAddress)]
        /* [Display(Name ="Email-Address")]*/
        public string email { get; set; }
        [Required]
        [RegularExpression(@"[9876]\d{9}", ErrorMessage = "Please Enter Validate Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile")]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string confirm { get; set; }
    }
}
