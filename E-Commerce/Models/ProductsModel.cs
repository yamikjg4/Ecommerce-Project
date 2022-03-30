using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class ProductsModel
    {
        [Key]
        [Display(Name = "Product Id")]
        public int product_id { get; set; }
        [Display(Name = "Selcet Category")]
        [Required(ErrorMessage = "Please Selcet Category")]
        /*       [RegularExpression("[a-zA-Z]+", ErrorMessage = "Alphabet is Not allowed")]*/
        public int? cat_id { get; set; }
        [Required(ErrorMessage = "Product Name is Required")]
        [Display(Name = "Product Name")]
        public string Product_name { get; set; }
        [Display(Name = "Product Price")]
        [Required(ErrorMessage = "Please Enter Product Price")]
        public float? product_price { get; set; }
        [Display(Name = "Product Description")]
        [Required(ErrorMessage = "Please Enter Product Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(800)]
        public string product_desc { get; set; }
        [Display(Name = "Product Quantity")]
        [Required(ErrorMessage = "Enter Product Qty")]
        public int? product_qty { get; set; }
        [Display(Name = "Product Image"), Required(ErrorMessage = "PLease Upload Product Image")]

        public IFormFile ImageFile { get; set; }
    }
}
