using Microsoft.EntityFrameworkCore;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;
using VinylApp.Domain.Models.VinylAppModels.GroupAggregate;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate.Auth;

namespace VinylApp.Infrastructure.Persistence.DbContexts
{
    public class VinylAppContext : DbContext
    {
        public VinylAppContext(DbContextOptions<VinylAppContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AlbumItem> AlbumItems { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(p => p.UserAuthorization)
                .WithOne(i => i.User)
                .HasForeignKey<AuthUser>(i => i.UserId);

            modelBuilder.Entity<User>()
                .HasMany(p => p.Albums)
                .WithOne(i => i.User)
                .HasForeignKey( i => i.UserId);
        }
    }
}
