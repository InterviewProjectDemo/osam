using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

using Xunit;
using AutoFixture;
using Moq;
using FluentAssertions;

using PortfolioManager.API.Controllers;
using PortfolioManager.BLL;
using PortfolioManager.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Results;

namespace PortfolioManager.API.Tests
{
    public class StockTraderControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IStockTrader> _stockTraderMock;
        private readonly StockTraderController _sut;

        public StockTraderControllerTest()
        {
            _fixture = new Fixture();
            _stockTraderMock = _fixture.Freeze<Mock<IStockTrader>>();
            _sut = new StockTraderController(_stockTraderMock.Object);
        }


        /************************************
         *        GetStocks Usecases        *
         * **********************************/
        [Fact]
        public void GetStocks_ShouldReturnOkResponse_WhenDataFound()
        {
            //Arrange
            var stocksMock = _fixture.Create<IEnumerable<Stock>>();
            _stockTraderMock.Setup(x => x.GetStocks()).Returns(stocksMock);

            //Act
            var result = _sut.GetStocks();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<IEnumerable<Stock>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(stocksMock.GetType());

            _stockTraderMock.Verify(m => m.GetStocks(), Times.Once());
        }

        [Fact]
        public void GetStocks_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            var stocksMock = _fixture.Create<IEnumerable<Stock>>();
            stocksMock = null;
            //List<Stock> response = null;
            _stockTraderMock.Setup(x => x.GetStocks()).Returns(stocksMock);

            //Act
            var result = _sut.GetStocks();

            //Assert
            result.Should().NotBeNull();
            result.Value.Should().BeNull();

            _stockTraderMock.Verify(m => m.GetStocks(), Times.Once());
        }

        /************************************
         *        BuyStock Usecases        *
         * **********************************/
        [Fact]
        public void BuyStock_ShouldReturnOkResponse_WhenNoException()
        {
            //Arrange
            var ticker = "ABC";
            var quantity = 10;
            Stock stockMock = new Stock { Ticker = ticker, Quantity = quantity };

            _stockTraderMock.Setup(x => x.BuyStock(ticker, quantity)).Verifiable();

            //Act
            var result = _sut.BuyStock(ticker, quantity);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Microsoft.AspNetCore.Mvc.OkResult>();

            _stockTraderMock.Verify(m => m.BuyStock(ticker, quantity), Times.Once());

        }


        [Fact]
        public void BuyStock_ShouldReturnException_WhenExceptionThown()
        {
            //Arrange
            var ticker = "ABC";
            var quantity = 10;
            var exceptionMock = new Exception();

            _stockTraderMock.Setup(x => x.BuyStock(ticker, quantity)).Throws(new Exception("Fail"));

            //Act
            var result = _sut.BuyStock(ticker, quantity);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>();
            BadRequestObjectResult actual = result as BadRequestObjectResult;
            Assert.Equal("Fail", actual.Value);
            Assert.Equal(400, actual.StatusCode);

            _stockTraderMock.Verify(m => m.BuyStock(ticker, quantity), Times.Once());
        }

        /************************************
         *  SellStock Usecases              *
         * **********************************/

        [Fact]
        public void SellStock_ShouldReturnOkResponse_WhenNoException()
        {
            //Arrange
            var ticker = "ABC";
            var quantity = 10;
            Stock stockMock = new Stock { Ticker = ticker, Quantity = quantity };

            _stockTraderMock.Setup(x => x.SellStock(ticker, quantity)).Verifiable();

            //Act
            var result = _sut.SellStock(ticker, quantity);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Microsoft.AspNetCore.Mvc.OkResult>();

            _stockTraderMock.Verify(m => m.SellStock(ticker, quantity), Times.Once());

        }

        [Fact]
        public void SellStock_ShouldReturnException_WhenExceptionThownByBLL()
        {
            //Arrange
            var ticker = "ABC";
            var quantity = 10;
            var exceptionMock = new Exception();
            string messageException = $"Stock {ticker} does not exist in the portfolio";
            //Note: Skipping validating $"Tried to sell {quantity} shares of {ticker} when portfolio only has {existingStock.Quantity}" as goal here to
            //      confirm, if message returned when exception thrown. Message content validation already checked in BLL test use-cases

            _stockTraderMock.Setup(x => x.SellStock(ticker, quantity)).Throws(new Exception(messageException));

            //Act
            var result = _sut.SellStock(ticker, quantity);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>();
            BadRequestObjectResult actual = result as BadRequestObjectResult;
            Assert.Equal(messageException, actual.Value);
            Assert.Equal(400, actual.StatusCode);

            _stockTraderMock.Verify(m => m.SellStock(ticker, quantity), Times.Once());

        }
    }
}
