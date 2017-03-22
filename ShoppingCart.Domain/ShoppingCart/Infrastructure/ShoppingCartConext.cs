using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.ShoppingCart;

namespace ShoppingCart.Domain.Data.Infrastructure
{
    public class ShoppingCartConext:DbContext
    {

        public ShoppingCartConext() : base("ShoppingCartConext")
        {
            Database.SetInitializer<ShoppingCartConext>(null);
        }
        
        public IDbSet<Basket> Baskets { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<Stock> Stocks { get; set; }
        public IDbSet<Checkout> Checkout { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

    }
}
