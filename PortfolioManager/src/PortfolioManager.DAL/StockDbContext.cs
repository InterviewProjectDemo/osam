using Microsoft.EntityFrameworkCore;

namespace PortfolioManager.DAL
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
