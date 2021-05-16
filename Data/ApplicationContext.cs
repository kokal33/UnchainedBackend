using Microsoft.EntityFrameworkCore;
using UnchainedBackend.Models;

namespace UnchainedBackend.Data
{
    public class ApplicationContext :DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PendingArtist> PendingArtists { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(x => x.PublicAddress)
                .ValueGeneratedOnAdd();
            builder.Entity<Bid>().Property(x => x.PublicAddress)
               .ValueGeneratedOnAdd();
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
