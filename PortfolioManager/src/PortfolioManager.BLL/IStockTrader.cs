using PortfolioManager.DAL;
using System.Collections.Generic;

namespace PortfolioManager.BLL
{
    public interface IStockTrader
    {
        void BuyStock(string ticker, double quantity);
        IEnumerable<Stock> GetStocks();
        void SellStock(string ticker, double quantity);
        bool BuyStockViaAddOrUpdateOption(string ticker, double quantity, Stock existingStock);
    }
}