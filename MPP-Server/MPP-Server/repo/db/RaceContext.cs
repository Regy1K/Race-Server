using Microsoft.EntityFrameworkCore;
using MPP_Server.model;

namespace MPP_Server.repo.db
{
    public class RaceContext : DbContext
    {
        public DbSet<Race> Races { get; set; } // You don't need the full namespace here.

        public RaceContext(DbContextOptions<RaceContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly map the Race entity to the 'Races' table
            modelBuilder.Entity<Race>().ToTable("Races");
            base.OnModelCreating(modelBuilder);
        }
    }
}