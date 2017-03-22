using System;
using Autofac;

namespace ShoppingCart.Infrastructure.Dependencies
{
    public class AutofacResolver : IResolver
    {
        private readonly IComponentContext _context;

        public AutofacResolver(IComponentContext context)
        {
            _context = context;
        }

        public T Resolve<T>()
        {
            return _context.Resolve<T>();
        }

        public object ResolveOptional(Type type)
        {
            return _context.ResolveOptional(type);
        }

        //public IEnumerable<T> ResolveAll<T>()
        //{
        //    return _context.Resolve<IEnumerable<T>>().ToList();
        //}
    }
}
