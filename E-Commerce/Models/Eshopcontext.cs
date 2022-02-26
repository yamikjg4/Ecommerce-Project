using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class Eshopcontext:IdentityDbContext<ApplicationUser>
    {
        public Eshopcontext()
        {

        }
        public Eshopcontext(DbContextOptions<Eshopcontext> options) : base(options)
        {

        }
        public DbSet<Category> tblcategory { get; set; }
        public DbSet<Product> tblproduct { get; set; }
    }
}
