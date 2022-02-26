using E_Commerce.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_Commerce.repositry
{
    public interface IProductRepositry
    {
        ICollection<Product> getdata();
        Task<Product> getdetail(int id);
    }
}