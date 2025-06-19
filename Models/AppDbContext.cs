using MatchReportNamespace;
using Microsoft.EntityFrameworkCore;

namespace WarApi.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MatchReport> MatchReports { get; set; }
        public DbSet<Player> Players { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MatchReport>()
                .HasOne(m => m.PlayerA)
                .WithMany()
                .HasForeignKey(m => m.PlayerAId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchReport>()
                .HasOne(m => m.PlayerB)
                .WithMany()
                .HasForeignKey(m => m.PlayerBId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
