using System;
using System.Collections.Generic;


using Xunit;
using AutoFixture;
using Moq;
using FluentAssertions;

using PortfolioManager.DAL;

namespace PortfolioManager.DAL.Tests
{
    public class StockRepositoryTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IStockRepository> _stockRepositoryMock;

        //public StockTraderControllerTest(IFixture fixture, Mock<IStockTrader> stockTraderMock, StockTraderController sut)
        public StockRepositoryTests()
        {
            _fixture = new Fixture();
            _stockRepositoryMock = _fixture.Freeze<Mock<IStockRepository>>();
        }

        [Fact]
        public void NotImplemented()
        {
            // Note: As there seems not additional logic besides Entitiry Framework's DBContext and its
            // functionality which would have been valided already by MS. Kept the setup available for TDD
            // once modifying Reposity/DAL
        }
    }
}
