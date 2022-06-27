using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioManager.DAL
{
    public class StockRepository
    {
        private readonly DbContextOptions<StockDbContext> _options;

        public StockRepository()
        {
            _options = new DbContextOptionsBuilder<StockDbContext>()
                .UseInMemoryDatabase("MyInMemoryDb")
                .Options;
        }

        public List<Stock> GetStocks()
        {
            using var db = new StockDbContext(_options);
            return db.Stocks.ToList();
        }

        public Stock GetStock(string ticker)
        {
            using var db = new StockDbContext(_options);
            return db.Stocks.Find(ticker);
        }

        public void AddStock(Stock stock)
        {
            using var db = new StockDbContext(_options);
            db.Stocks.Add(stock);
            db.SaveChanges();
        }

        public void RemoveStock(string ticker)
        {
            using var db = new StockDbContext(_options);
            var stock = db.Stocks.Find(ticker);
            db.Stocks.Remove(stock);
            db.SaveChanges();
        }

        public void UpdateQuantity(string ticker, double quantity)
        {
            using var db = new StockDbContext(_options);
            var stock = db.Stocks.Find(ticker);
            stock.Quantity = quantity;
            db.SaveChanges();
        }
    }
}
