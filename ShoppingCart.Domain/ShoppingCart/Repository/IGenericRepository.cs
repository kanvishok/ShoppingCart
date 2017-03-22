using System;
using System.Linq;
using ShoppingCart.Domain.Data.Entity;

namespace ShoppingCart.Domain.Data.Repository
{
    public interface IGenericRepository<T> where T : class, IEntityBase, new()
    {
        IQueryable<T> GetAll();
        int Add(T entity);
        int Edit(T entity);
        void Delete(T entity);
        T GetSingle(int id);
        IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
    }
}
