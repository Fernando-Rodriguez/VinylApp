using System;
using System.Linq;
using System.Threading.Tasks;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;
using VinylApp.Infrastructure.Persistence.DbContexts;
using VinylApp.Infrastructure.Services.SpotifyService;

namespace VinylApp.Infrastructure.Persistence.Repository
{
    public class AlbumRepo : BaseRepo<AlbumItem>, IAlbumRepo
    {
        private readonly ISpotifyRequestClient _spotifyRequest;

        public AlbumRepo(
            VinylAppContext context,
            ISpotifyRequestClient spotifyRequest) : base(context)
        {
            _spotifyRequest = spotifyRequest;
        }

        public override async Task Add(AlbumItem album)
        {
            var albumUrl = await DataMatcher(album);
            album.UpdateArtworkUrl(albumUrl);
            _context.AlbumItems.Add(album);
        }

        private async Task<string> DataMatcher(AlbumItem album)
        {
            var spotifyRes = await _spotifyRequest.Search(album);
            if (spotifyRes.Albums == null) throw new Exception("Spotify url null");
            var imageUrl = spotifyRes
                .Albums
                .Items
                .Where(a => string.Equals(a.Artists.First().Name, album.ArtistName, StringComparison.CurrentCultureIgnoreCase))
                .Select(a => a.Images)
                .FirstOrDefault()
                ?.FirstOrDefault()
                ?.Url;

            return imageUrl;
        }
    }
}