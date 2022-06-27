using Microsoft.AspNetCore.Mvc;
using PortfolioManager.BLL;
using PortfolioManager.DAL;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PortfolioManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockTraderController : ControllerBase
    {

        private readonly IStockTrader _stockTrader;
        
        public StockTraderController(IStockTrader stockTrader)
        {
            _stockTrader = stockTrader;
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Stock>> GetStocks()
        {
            IEnumerable<Stock> stocks = _stockTrader.GetStocks();
            List<Stock> lstStocks = new List<Stock>() { };
            if(stocks != null)
            {
                //Ths additional logic to confirm stock(s) exists before sending otherwise return not found.
                foreach (var stock in stocks)
                {
                    lstStocks.Add(stock);
                    if(lstStocks.Count > 1) { break;  } 
                }

                if (lstStocks.Count > 0)
                {
                    return Ok(stocks);
                }
                else
                {
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
                }
            }
            else {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound);
            }
            
            //return Ok(stocks);
            //IEnumerable<Stock> stocks = StockTrader.GetStocks();
            //return Ok(stocks);
        }

        [HttpPost("[action]")]
        public IActionResult BuyStock(string ticker, double quantity)
        {
            try
            {
                _stockTrader.BuyStock(ticker, quantity);
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
                _stockTrader.SellStock(ticker, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
