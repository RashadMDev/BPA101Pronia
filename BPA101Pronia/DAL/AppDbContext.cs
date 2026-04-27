using BPA101Pronia.Models;
using Microsoft.EntityFrameworkCore;

namespace BPA101Pronia.DAL
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=B3-0\\SQLEXPRESS;Database=BPA101ProniaDB;Trusted_Connection=True;TrustServerCertificate=true");
        }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
