using ShoppingCart.Domain.ShoppingCart;

namespace ShoppingCart.Domain.Data.Entity
{
    public class Item : IEntityBase
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public int BasketId { get; set; }
        public virtual Product Products { get; set; }
        public virtual Basket Baskets { get; set; }
    }
}
