using PortfolioManager.DAL;
using System;
using System.Collections.Generic;

namespace PortfolioManager.BLL
{
    public class StockTrader
    {
        public static IEnumerable<Stock> GetStocks()
        {
            var repo = new StockRepository();
            return repo.GetStocks();
        }

        public static void BuyStock(string ticker, double quantity)
        {
            var repo = new StockRepository();
            var existingStock = repo.GetStock(ticker);

            if (existingStock == null)
            {
                var stock = new Stock()
                {
                    Ticker = ticker,
                    Quantity = quantity
                };
                repo.AddStock(stock);
            }
            else
            {
                var newQuantity = existingStock.Quantity + quantity;
                repo.UpdateQuantity(ticker, newQuantity);
            }
        }

        public static void SellStock(string ticker, double quantity)
        {
            var repo = new StockRepository();
            var existingStock = repo.GetStock(ticker);

            if (existingStock == null)
                throw new Exception($"Stock {ticker} does not exist in the portfolio");

            if (existingStock.Quantity < quantity)
                throw new Exception($"Tried to sell {quantity} shares of {ticker} when portfolio only has {existingStock.Quantity}");

            if (existingStock.Quantity == quantity)
            {
                repo.RemoveStock(ticker);
            }
            else
            {
                var newQuantity = existingStock.Quantity - quantity;
                repo.UpdateQuantity(ticker, newQuantity);
            }
        }
    }
}
