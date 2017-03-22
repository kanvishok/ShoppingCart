using System;
using System.Collections.Generic;

namespace ShoppingCart.Domain.Data.Entity
{
    public class Product:IEntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }



    }
}