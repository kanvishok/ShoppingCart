namespace ShoppingCart.Domain.Data.Entity
{
    public class Stock : IEntityBase
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Products { get; set; }
        public int AvailableStock { get; set; }
        public int EstimatedStock { get; set; }

    }
}
