﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class singupmodel
    {
        [Required]
        [RegularExpression("[a-zA-Z]+",ErrorMessage ="Alphabet is Not allowed")]
        [Display(Name = "User Name")]
        public string Name { get; set; }
        [Required,EmailAddress]
        [DataType(DataType.EmailAddress)]
       /* [Display(Name ="Email-Address")]*/
        public string email { get; set; }
        [Required]
        [RegularExpression(@"[9876]\d{9}",ErrorMessage ="Please Enter Validate Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile")]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
