using Microsoft.EntityFrameworkCore;
using MusicStore.Entities.Info;
using System.Reflection;

namespace MusicStore.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customizing the migrations ...
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Ignore<ConcertInfo>();
            modelBuilder.Entity<ConcertInfo>().HasNoKey();
        }

        // Entities to tables
        //public DbSet<Genre> Genres { get; set; }
    }
}
