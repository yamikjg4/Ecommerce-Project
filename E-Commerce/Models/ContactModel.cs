using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class ContactModel
    {
        [Required]
        public string uname { get; set; }
        [EmailAddress]
        [Required]
        public string email { get; set; }
        [Required]
        [RegularExpression(@"[9876]\d{9}", ErrorMessage = "Please Enter Validate Mobile Number")]
        public string mobile { get; set; }
        public string comment { get; set; }
    }
}
