using System;
using System.Threading.Tasks;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;
using VinylApp.Domain.Models.VinylAppModels.GroupAggregate;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate;
using VinylApp.Infrastructure.Persistence.DbContexts;
using VinylApp.Infrastructure.Persistence.Repository;
using VinylApp.Infrastructure.Services.SpotifyService;

namespace VinylApp.Infrastructure.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private VinylAppContext _context;

        public UnitOfWork(VinylAppContext context, ISpotifyRequestClient spotifyRequest)
        {
            _context = context;
            Albums = new AlbumRepo(_context, spotifyRequest);
            Users = new UserRepo(_context);
            Group = new GroupRepo(_context);
        }

        public IAlbumRepo Albums { get; }
        public IUserRepo Users { get; }
        public IGroupRepo Group { get; }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_context == null) return;
            _context.Dispose();
            _context = null;
        }
    }
}
