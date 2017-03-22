using System;
using System.Threading.Tasks;
using ShoppingCart.Infrastructure.Dependencies;

namespace ShoppingCart.Infrastructure.Core
{
    public class QueryDispatcher:IQueryDispatcher
    {
        private readonly IResolver _resolver;

        public QueryDispatcher(IResolver resolver)
        {
            _resolver = resolver;
        }
        

        public Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {

            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var queryHandler = _resolver.ResolveOptional(typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult))) as IQueryHandler<TQuery, TResult>;

            if (queryHandler == null)
                throw new ArgumentException("No async handler found for query: " + query.GetType().FullName);

            return queryHandler.RetrieveAsync(query);
        }
    }
}