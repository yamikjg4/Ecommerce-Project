﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class tblAddress
    {
        [Key]
        public int ad_id{ get; set; }
        [Required,Display(Name ="Name")]

       /* [DataType(DataType.Na)]*/
      /*  [RegularExpression(@"\s+[a-zA-Z]", ErrorMessage = "Alphabet is Not allowed")]*/
        public string name { get; set; }
        [Required,Display(Name="Mobile No")]
        [RegularExpression(@"[9876]\d{9}", ErrorMessage = "Please Enter Validate Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        [Display(Name ="Pincode")]
        public int? pincode { get; set; }
        [Required,Display(Name ="Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string Id { get; set; }
        public ApplicationUser user { get; set; }
        public virtual ICollection<TBLorder> ord { get; set; }
    }
}
