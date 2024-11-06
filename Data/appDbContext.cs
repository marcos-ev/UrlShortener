using Microsoft.EntityFrameworkCore;
using urlshorter.Models;

namespace urlshorter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Url> Urls { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccessLog>()
                .HasOne(al => al.Url)
                .WithMany()
                .HasForeignKey(al => al.UrlId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
