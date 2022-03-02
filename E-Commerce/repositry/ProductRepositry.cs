using E_Commerce.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.repositry
{
    public class ProductRepositry : IProductRepositry
    {
        private readonly Eshopcontext _context;
        public ProductRepositry(Eshopcontext context)
        {
            _context = context;
        }
        public ICollection<Product> getdata()
        {
            return _context.tblproduct.ToList();
        }
        public async Task<Product> getdetail(int id)
        {

            var result = await _context.tblproduct.FindAsync(id);


            return result;
        }
    }
}
