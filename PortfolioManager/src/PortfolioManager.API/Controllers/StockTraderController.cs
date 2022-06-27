using Microsoft.AspNetCore.Mvc;
using PortfolioManager.BLL;
using PortfolioManager.DAL;
using System;
using System.Collections.Generic;

namespace PortfolioManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockTraderController : ControllerBase
    {
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Stock>> GetStocks()
        {
            IEnumerable<Stock> stocks = StockTrader.GetStocks();
            return Ok(stocks);
        }

        [HttpPost("[action]")]
        public IActionResult BuyStock(string ticker, double quantity)
        {
            try
            {
                StockTrader.BuyStock(ticker, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public IActionResult SellStock(string ticker, double quantity)
        {
            try
            {
                StockTrader.SellStock(ticker, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
