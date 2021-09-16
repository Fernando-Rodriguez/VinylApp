using System.Threading.Tasks;
using VinylApp.Domain.DTOs.SpotifyDTOs;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;

namespace VinylApp.Infrastructure.Services.SpotifyService
{
    public interface ISpotifyRequestClient
    {
        Task<SpotifyAlbum> Search(AlbumItem album);
    }
}