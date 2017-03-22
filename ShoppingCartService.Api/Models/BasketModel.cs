namespace ShoppingCart.Api.Models
{
    public class BasketModel
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}