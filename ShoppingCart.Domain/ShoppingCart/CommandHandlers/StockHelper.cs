using System.Data.Entity;
using System.Threading.Tasks;
using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.Data.Repository;

namespace ShoppingCart.Domain.ShoppingCart.CommandHandlers
{
    public  class StockHelper
    {
        public static void UpdateStock(IGenericRepository<Stock> stockRepository, int productId, int quantity)
        {
            //Assuming that always there will be a stock entry for the product. 
            //Even the stock is not available then there will be stock entry with zero stock
            var stock = Task.Run(() => stockRepository.FindBy(s => s.ProductId == productId).FirstOrDefaultAsync()).Result;

            if (stock != null)
            {
                stock.AvailableStock = stock.AvailableStock - quantity;
                stock.EstimatedStock = stock.EstimatedStock - quantity;
                stockRepository.Edit(stock);
            }
        }
    }
}
