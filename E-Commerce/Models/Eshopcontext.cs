using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Models
{
    public class Eshopcontext : IdentityDbContext<ApplicationUser>
    {
        public Eshopcontext()
        {

        }
        public Eshopcontext(DbContextOptions<Eshopcontext> options) : base(options)
        {

        }
        public DbSet<Category> tblcategory { get; set; }
        public DbSet<Product> tblproduct { get; set; }
        public DbSet<tblAddress> tblAddresses { get; set; }
    }
}
