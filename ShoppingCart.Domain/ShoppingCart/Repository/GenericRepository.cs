using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.Data.Infrastructure;

namespace ShoppingCart.Domain.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntityBase, new()
    {
        private ShoppingCartConext _dbContext;

        protected IDbFactory DbFactory
        {
            get;
            private set;
        }
        public GenericRepository(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
        }

        protected ShoppingCartConext DbContext => _dbContext ?? (_dbContext = DbFactory.CreateDbContext());

        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }

        public virtual int Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
            return entity.Id;
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        public T GetSingle(int id)
        {
            return DbContext.Set<T>().SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate);
        }

        public int Edit(T entity)
        {

            // DbContext.Set<T>().AddOrUpdate(entity);
            DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
            dbEntityEntry.State = EntityState.Modified;

            return entity.Id;
        }
    }
}