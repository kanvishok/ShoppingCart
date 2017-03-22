using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Core
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }

}