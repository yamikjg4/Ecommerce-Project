using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Models
{
    public class Cart
    {
        public List<Product> cartproduct { get; set; }
        public List<int> cartprouctid { get; set; }
        public int proid { get; set; }
       
        public int qty { get; set; }
      
    }
}
