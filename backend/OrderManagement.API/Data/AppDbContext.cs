using Microsoft.EntityFrameworkCore;
using OrderManagement.API.Models;
using AppDbContext = OrderManagement.API.Data.AppDbContext;

namespace OrderManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}