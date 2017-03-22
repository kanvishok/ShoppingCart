using System;
using ShoppingCart.Domain.ShoppingCart.Commands;
using ShoppingCart.Infrastructure.Core;
using System.Collections.Generic;
using System.Linq;
using ShoppingCart.Domain.Data.Entity;

namespace ShoppingCart.Domain.ShoppingCart
{
    public class Basket : Aggregate, IEntityBase
    {
        public int CustomerId { get; private set; }
        public virtual ICollection<Item> Items { get; private set; }
        public DateTime LastUpdate { get; private set; }
        public bool IsActive { get; set; }
        public Basket() { }
        
        private Basket(EditBasketItemQuantityCommand command)
        {
            Id = command.BasketId;
            CustomerId = command.CustomerId;
            Items = new List<Item> { new Item { ProductId = command.ProductId, BasketId = command.BasketId, Quantity = command.Quantity } };
            LastUpdate = DateTime.Now;
        }

        public static Basket DisableBasket(Basket basket)
        {
            basket.IsActive = false;
            return basket;

        }

        public static Basket AddToBasket(AddToBasketCommand command, Basket basket)
        {
            if (basket != null)
            {

                basket.Items.Add(new Item
                {
                    ProductId = command.ProductId,
                    Quantity = command.Quantity,
                    BasketId = basket.Id
                });
            }
            else
            {
                basket = new Basket
                {
                    Items =
                        new List<Item>
                        {
                            new Item
                            {
                                ProductId = command.ProductId,
                                BasketId = command.BasketId,
                                Quantity = command.Quantity
                            }
                        }
                };
            }
            basket.LastUpdate = DateTime.Now;
            basket.CustomerId = command.CustomerId;
            basket.IsActive = true;
            return basket;
        }
        public static Basket EditBasketItemQuantity(EditBasketItemQuantityCommand command, Basket basket)
        {
            var itemToUpdate =
                   basket.Items.FirstOrDefault(i => i.ProductId == command.ProductId);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = command.Quantity;
            }
            basket.LastUpdate = DateTime.Now;
            basket.IsActive = true;
            return basket;
        }

        public int Id { get; set; }
    }
}
