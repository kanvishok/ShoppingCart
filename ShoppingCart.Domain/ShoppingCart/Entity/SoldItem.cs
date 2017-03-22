using ShoppingCart.Domain.Data.Entity;

namespace ShoppingCart.Domain.ShoppingCart.Entity
{
    public class SoldItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public int CheckoutId { get; set; }
        public decimal PriceDuringCheckout { get; set; }
        public virtual Product Products { get; set; }
        public virtual Checkout Checkout { get; set; }
    }
}
