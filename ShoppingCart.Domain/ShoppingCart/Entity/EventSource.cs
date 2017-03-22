using ShoppingCart.Domain.Data.Entity;

namespace ShoppingCart.Domain.ShoppingCart.Entity
{
    public class EventSource:IEntityBase
    {
        public int Id { get; set; }
        public int AggregateId { get; set; }
        public string Event { get; set; }

    }
}
