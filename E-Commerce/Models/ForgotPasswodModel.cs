using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
