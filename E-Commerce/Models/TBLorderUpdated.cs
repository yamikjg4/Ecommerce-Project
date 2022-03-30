using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Models
{
    public class TBLorderUpdated
    {
        [Key]
        /*[DatabaseGenerated(DatabaseGeneratedOption.Identity)]*/
        public int orderid { get; set; }
        [ForeignKey("Address")]
        [Required, Display(Name = "Address")]

        public int? ad_id { get; set; }
        [ForeignKey("prd")]
        public int? product_id { get; set; }
        /*[ForeignKey("productid")]*/
        public int? qtys { get; set; }

        [ForeignKey("user")]
        public string Id { get; set; }
        public int totalpay { get; set; }
        public string status { get; set; }
        public string payment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "MM / dd / yyyy hh: mm tt")]
        public DateTime date { get; set; }
        public virtual tblAddress Address { get; set; }
        public virtual Product prd { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}
