using System;

namespace ShoppingCart.Domain.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        ShoppingCartConext CreateDbContext();
    }
}
