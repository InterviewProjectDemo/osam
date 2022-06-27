using System;
using System.Collections.Generic;


using Xunit;
using AutoFixture;
using Moq;
using FluentAssertions;

using PortfolioManager.DAL;
using PortfolioManager.BLL;

namespace PortfolioManager.BLL.Tests
{
    public class StockTraderServicesTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IStockRepository> _stockRepositoryMock;
        private readonly Mock<IStockTrader> _stockTraceMock;
        private readonly StockTrader _sut;

        //public StockTraderControllerTest(IFixture fixture, Mock<IStockTrader> stockTraderMock, StockTraderController sut)
        public StockTraderServicesTest()
        {
            _fixture = new Fixture();
            _stockRepositoryMock = _fixture.Freeze<Mock<IStockRepository>>();
            _stockTraceMock = _fixture.Freeze<Mock<IStockTrader>>();
            _sut = new StockTrader(_stockRepositoryMock.Object);
        }

        /************************************
         *        GetStocks Usecases        *
         * **********************************/
        [Fact]
        public void GetStocks_ShouldListOfStocks_WhenStockFound()
        {
            //Arrange
            var stocksMock = _fixture.Create<List<Stock>>();
            _stockRepositoryMock.Setup(x => x.GetStocks()).Returns(stocksMock);

            //Act
            var result = _sut.GetStocks();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<Stock>>();
           
            _stockRepositoryMock.Verify(m => m.GetStocks(), Times.Once());
        }

        [Fact]
        public void GetStocks_ShouldNull_WhenNoStockFound()
        {
            //Arrange
            var stocksMock = _fixture.Create<List<Stock>>();
            stocksMock = null;
            _stockRepositoryMock.Setup(x => x.GetStocks()).Returns(stocksMock);

            //Act
            var result = _sut.GetStocks();

            //Assert
            Assert.Null(result);

            _stockRepositoryMock.Verify(m => m.GetStocks(), Times.Once());
        }


        /************************************
         *        BuyStock Usecases        *
         * **********************************/
        [Fact]
        public void BuyStockAddOrUpdateOption_ShouldReturnTrue_WhenGivenStockNotExisting()
        {
            //Note: BuyStockAddOrUpdateOption return true? Stock purchased via add option : Stock purchased via update option. This method was added in refactoring

            //Arrange
            var ticker = It.IsAny<string>();
            var quantity = It.IsAny<double>();
            Stock stockMock = new Stock { Ticker = ticker, Quantity=quantity };
            stockMock = null;

            //Act
            var result = _sut.BuyStockViaAddOrUpdateOption(ticker, quantity, stockMock);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void BuyStockAddOrUpdateOption_ShouldReturnFalse_WhenGivenStoctExisting()
        {
            //Note: Same as .BuyStockAddOrUpdateOption_ShouldReturnTrue_WhenGivenStockNotExisting()'s note. Also use It.IsAny<T>() has null value potential

            //Arrange
            string ticker = "ABC";
            double quantity = 10;
            Stock stockMock = new Stock { Ticker = ticker, Quantity = quantity };

            //Act
            var result = _sut.BuyStockViaAddOrUpdateOption(ticker, quantity, stockMock);

            //Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void BuyStock_ShouldAddStock_WhenGivenStockNotExisting()
        {
            //Arrange
            string ticker = "ABC";  
            double quantity = 10;   
            Stock stockMock = new Stock { Ticker = ticker, Quantity = quantity };
            _stockRepositoryMock.Setup(x => x.GetStock(ticker)).Returns<Stock>(null);
            
            //Act
            _sut.BuyStock(ticker, quantity);
            
            //Assert
            _stockRepositoryMock.Verify(m => m.GetStock(ticker), Times.Once());
            _stockRepositoryMock.Verify(m => m.AddStock(It.IsAny<Stock>()), Times.AtLeastOnce());
        }

        [Fact]
        public void BuyStock_ShouldUpdateStock_WhenGivenStockExisting()
        {
            //Arrange
            string ticker = "ABC";  
            double quantity = 10;  
            Stock stockMock = new Stock { Ticker = ticker, Quantity = quantity };
            _stockRepositoryMock.Setup(x => x.GetStock(ticker)).Returns(stockMock);
            
            //Act
            _sut.BuyStock(ticker, quantity);

            //Assert
            _stockRepositoryMock.Verify(m => m.GetStock(ticker), Times.Once());
            _stockRepositoryMock.Verify(m => m.UpdateQuantity(It.IsAny<string>(), It.IsAny<double>()), Times.AtLeastOnce());
        }


        /************************************
         *        SellStock Usecases        *
         * **********************************/
        [Fact] // Logic: GetStock() returns null for Stock to sell
        public void SellStock_ShouldThowException_WhenGivenStockNotExisting()
        {
            //Arrange
            string ticker = "ABC";
            double quantity = 10;
            string messageException = "";
           
            //Act
            //Note: Because method has not return hence using try catch to manage exception. 
            try {
                _sut.SellStock(ticker, quantity);
            }
            catch(Exception ex){
                messageException = ex.Message;
            }

            //Assert
            Assert.Contains(ticker, messageException);
            Assert.Equal($"Stock {ticker} does not exist in the portfolio", messageException);

            _stockRepositoryMock.Verify(m => m.GetStock(ticker), Times.Once());
        }


        [Fact] //Logic: existingStock.Quantity < quantity
        public void SellStock_ShouldThowException_WhenExistingStockQtyLTQtyToSell()
        {
            //Arrange
            string ticker = "ABC";
            double quantity = 10;
            string messageException = "";
            Stock mockStock = new Stock { Ticker = ticker, Quantity = quantity - 1 };
            _stockRepositoryMock.Setup(x => x.GetStock(ticker)).Returns(mockStock);

            //Act
            //Note: Because method has not return hence using try catch to manage exception. 
            try
            {
                _sut.SellStock(ticker, quantity);
            }
            catch (Exception ex)
            {
                messageException = ex.Message;
            }

            //Assert
            Assert.Contains(ticker, messageException);
            Assert.Equal($"Tried to sell {quantity} shares of {ticker} when portfolio only has {mockStock.Quantity}", messageException);

            _stockRepositoryMock.Verify(m => m.GetStock(ticker), Times.Once());
        }

        
        [Fact] //Logic: existingStock.Quantity == quantity
        public void SellStock_ShouldPerformRemoveStock_WhenExistingStockQtyEQQtyToSell()
        {
            //Arrange
            string ticker = "ABC";
            double quantity = 10;
            Stock mockStock = new Stock { Ticker = ticker, Quantity = quantity};
            _stockRepositoryMock.Setup(x => x.GetStock(ticker)).Returns(mockStock);

            //Act
            _sut.SellStock(ticker, quantity);

            //Assert
            _stockRepositoryMock.Verify(m => m.RemoveStock(ticker), Times.Once());
        }


        [Fact]
        public void SellStock_ShouldUpdateQuantity_WhenExistingStockQtyGTQtyToSell()
        {
            //Arrange
            string ticker = "ABC";
            double quantity = 10;
            Stock mockStock = new Stock { Ticker = ticker, Quantity = quantity + 1 };
            _stockRepositoryMock.Setup(x => x.GetStock(ticker)).Returns(mockStock);

            //Act
            _sut.SellStock(ticker, quantity);

            //Assert
            _stockRepositoryMock.Verify(m => m.UpdateQuantity(ticker, mockStock.Quantity - quantity), Times.Once());
        }
    }
}
