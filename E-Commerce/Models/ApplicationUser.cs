using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace E_Commerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string fname { get; set; }
       
        public virtual ICollection<TBLorder> ord { get; set; }
    }
}
