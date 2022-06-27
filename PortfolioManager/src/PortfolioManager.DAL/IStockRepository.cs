using System.Collections.Generic;

namespace PortfolioManager.DAL
{
    public interface IStockRepository
    {
        void AddStock(Stock stock);
        Stock GetStock(string ticker);
        List<Stock> GetStocks();
        void RemoveStock(string ticker);
        void UpdateQuantity(string ticker, double quantity);
    }
}