namespace ShoppingCart.Domain.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private ShoppingCartConext _dbContext;
        public ShoppingCartConext CreateDbContext()
        {
            return _dbContext ?? (_dbContext = new ShoppingCartConext());
        }

        protected override void DisposeCore()
        {
            _dbContext?.Dispose();
        }
    }
}