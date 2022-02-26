using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
   
    public class Product
    {
        
        [Key]
        [Display(Name ="Product Id")]
        public int product_id { get; set; }
        [Display(Name = "Category Id")]
        [Required]
        /*[ForeignKey("cat_id")]*/
        public int cat_id { get; set; }
        [Display(Name ="Product Name")]
        [Required]
        public string Product_name { get; set; }
        [Display(Name ="Product Price")]
        [Required]
        public float? product_price { get; set; }
        [Display(Name = "Product Description")]
        [StringLength(800)]
        [Required]
        public string product_desc { get; set; }
        [Display(Name = "Product Quantity")]
        [Required]
        public int? product_qty { get; set; }
        [Display(Name = "Product Image")]
        [Required]
        public string ImageFile { get; set; }
        public Category category { get; set; }
    }
}
