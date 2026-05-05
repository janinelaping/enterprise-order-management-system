using Microsoft.EntityFrameworkCore;
using OrderManagement.API.Models;
using AppDbContext = OrderManagement.API.Data.AppDbContext;
using Microsoft.AspNetCore.Identity;

namespace OrderManagement.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
		public DbSet<User> Users { get; set; }
    }
}