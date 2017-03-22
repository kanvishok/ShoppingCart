using System;

namespace ShoppingCart.Infrastructure.Dependencies
{
    public  interface IResolver
    {
        T Resolve<T>();
        object ResolveOptional(Type type);
    }
}