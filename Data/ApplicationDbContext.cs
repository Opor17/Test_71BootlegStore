using _71BootlegStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _71BootlegStore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        #region master data

        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<ProductManagement> ProductManagement { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<CartDetail> CartDetail { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
