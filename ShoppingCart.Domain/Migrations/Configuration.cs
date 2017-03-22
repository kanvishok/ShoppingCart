using ShoppingCart.Domain.Data.Entity;
using ShoppingCart.Domain.Data.Infrastructure;

namespace ShoppingCart.Domain.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppingCartConext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShoppingCartConext context)
        {
            context.Products.AddOrUpdate(p => p.Name, AddProducts());
            context.Stocks.AddOrUpdate(s => s.ProductId, AddStocks());
        }
        private Stock[] AddStocks()
        {
            Stock[] stocks = new Stock[]
            {
                new Stock {ProductId = 1, AvailableStock = 5,EstimatedStock = 5},
                new Stock {ProductId = 2, AvailableStock = 10,EstimatedStock = 10},
                new Stock {ProductId = 3, AvailableStock = 10,EstimatedStock = 10},
                new Stock {ProductId = 4, AvailableStock = 3,EstimatedStock = 3},
                new Stock {ProductId = 5, AvailableStock = 20,EstimatedStock = 20},
            };

            return stocks;

        }

        private Product[] AddProducts()
        {
            Product[] products = new Product[]
            {
                new Product() {Name = "Apples", Description = "Fruit", Price = 2.5M},
                new Product() {Name = "Bread", Description = "Loaf", Price = 1.35M},
                new Product() {Name = "Oranges", Description = "Fruit", Price = 2.99M},
                new Product() {Name = "Milk", Description = "Milk",Price = 2.07M},
                new Product() {Name = "Chocalate", Description = "Bars",Price = 1.79M},
            };
            return products;
        }
    }

}
