using ShoppingCart.Domain.Data.Infrastructure;

namespace HomeCinema.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private ShoppingCartConext dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public ShoppingCartConext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.CreateDbContext()); }
        }

        public void Commit()
        {
            DbContext.Commit();
        }
    }
}
