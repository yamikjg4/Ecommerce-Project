using System.Collections.Generic;

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
