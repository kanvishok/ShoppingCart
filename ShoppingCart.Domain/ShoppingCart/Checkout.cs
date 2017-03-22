using System.Collections.Generic;
using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.ShoppingCart.Entity;
using ShoppingCart.Infrastructure.Core;

namespace ShoppingCart.Domain.ShoppingCart
{
    public sealed class Checkout : Aggregate, IEntityBase
    {
        public int Id { get; set; }
        public ICollection<SoldItem> SoldItems { get; set; }
        public Checkout() { }
        public int BasketId { get; private set; }

        public int CustomerId { get; private set; }
        public Checkout(Basket basket)
        {
            SoldItems = new List<SoldItem>();
            foreach (var item in basket.Items)
            {
                SoldItems.Add(new SoldItem
                {
                    ProductId = item.ProductId,
                    PriceDuringCheckout = item.Products.Price,
                    Quantity = item.Quantity,
                    CheckoutId = Id
                });
            }
            BasketId = basket.Id;
            CustomerId = basket.Id;
        }

        //AutoMapper 
        //SoldItems = Mapper.Map<ICollection<Item>, ICollection<SoldItem>>(basket.Items);

    }

}

