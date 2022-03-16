using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models
{
    public class Category
    {

        [Key]
        /*[Required]*/
        [Display(Name = "Category Id")]
        public int cat_id { get; set; }
        [Display(Name = "Category Name")]
        [Required]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Alphabet is Not allowed")]
        public string category_name { get; set; }

    }
}
