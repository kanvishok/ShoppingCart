using System.Collections.Generic;
using ShoppingCart.Domain.Data.Entity;

namespace ShoppingCart.Query.Models
{
    public class Items
    {
        public Items()
        {
            
        }
        public IEnumerable<Product> AllItems { get; set; }
    }
}
