using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class Category
    {
        
        [Key]
        /*[Required]*/
        [Display(Name ="Category Id")]
        public int cat_id { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        public string category_name { get; set; }
        
    }
}
