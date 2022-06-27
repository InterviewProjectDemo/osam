using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioManager.DAL
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _dbContext;

        public StockRepository(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Stock> GetStocks()
        {
            return _dbContext.Stocks.ToList();
        }

        public Stock GetStock(string ticker)
        {
            return _dbContext.Stocks.Find(ticker);
        }

        public void AddStock(Stock stock)
        {
            _dbContext.Stocks.Add(stock);
            _dbContext.SaveChanges();
        }

        public void RemoveStock(string ticker)
        {
            var stock = _dbContext.Stocks.Find(ticker);
            _dbContext.Stocks.Remove(stock);
            _dbContext.SaveChanges();
        }

        public void UpdateQuantity(string ticker, double quantity)
        {
            var stock = _dbContext.Stocks.Find(ticker);
            stock.Quantity = quantity;
            _dbContext.SaveChanges();
        }
    }
}
