using PortfolioManager.DAL;
using System;
using System.Collections.Generic;

namespace PortfolioManager.BLL
{
    public class StockTrader : IStockTrader
    {
        public readonly IStockRepository _stockRepository;

        public StockTrader(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository ?? throw new ArgumentNullException(nameof(stockRepository));
        }

        public IEnumerable<Stock> GetStocks()
        {
            return _stockRepository.GetStocks();
        }

        public void BuyStock(string ticker, double quantity)
        {
            var existingStock = _stockRepository.GetStock(ticker);
            BuyStockViaAddOrUpdateOption(ticker, quantity, existingStock);
        }

        public void SellStock(string ticker, double quantity)
        {
            //var repo = new StockRepository();
            var existingStock = _stockRepository.GetStock(ticker);

            if (existingStock == null)
                throw new Exception($"Stock {ticker} does not exist in the portfolio");

            if (existingStock.Quantity < quantity)
                throw new Exception($"Tried to sell {quantity} shares of {ticker} when portfolio only has {existingStock.Quantity}");

            if (existingStock.Quantity == quantity)
            {
                _stockRepository.RemoveStock(ticker);
            }
            else
            {
                var newQuantity = existingStock.Quantity - quantity;
                _stockRepository.UpdateQuantity(ticker, newQuantity);
            }
        }

        /********************************************************************************************
         * Note: Purpose of this additional method in refactoring process just to show that a method
         * with calling multiple different down steam methods can be separated to validate each method
         * logic distinguishly and cover more scenarios in each of those methods. Based on use case of
         * BuyStock it could be redundant as methods part of the if else logic also covered in different
         * Test cases (e.g., Refer to BuyStock_ShouldUpdateStock_WhenGivenStockExisting() &
         * BuyStock_ShouldAddStock_WhenGivenStockNotExisting())
         * ******************************************************************************************/
        public bool BuyStockViaAddOrUpdateOption(string ticker, double quantity, Stock existingStock)
        {
            if (existingStock == null)
            {
                var stock = new Stock()
                {
                    Ticker = ticker,
                    Quantity = quantity
                };
                _stockRepository.AddStock(stock);
                return true;
            }
            else
            {
                var newQuantity = existingStock.Quantity + quantity;
                _stockRepository.UpdateQuantity(ticker, newQuantity);
                return false;
            }
        }
    }
}
