using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure.Core
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> RetrieveAsync(TQuery query);
    }

}