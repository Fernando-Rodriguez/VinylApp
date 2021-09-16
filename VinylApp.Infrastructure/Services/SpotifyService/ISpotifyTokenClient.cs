using System.Threading.Tasks;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate.Auth;

namespace VinylApp.Infrastructure.Services.SpotifyService
{
    public interface ISpotifyTokenClient
    {
        Task<SpotifyToken> RetrieveToken();
    }
}