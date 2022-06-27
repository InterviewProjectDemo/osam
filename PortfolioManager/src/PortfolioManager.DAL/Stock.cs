using System.ComponentModel.DataAnnotations;

namespace PortfolioManager.DAL
{
    public class Stock
    {
        [Key]
        public string Ticker { get; set; }
        public double Quantity { get; set; }
    }
}
