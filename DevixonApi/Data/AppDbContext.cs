using DevixonApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevixonApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(i => i.Image);
            
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Image>().ToTable("Images");
        }
    }
}