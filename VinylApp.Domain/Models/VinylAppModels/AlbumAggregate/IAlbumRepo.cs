using System.Threading.Tasks;
using VinylApp.Domain.Models.CoreModels;

namespace VinylApp.Domain.Models.VinylAppModels.AlbumAggregate
{
    public interface IAlbumRepo : IBaseRepo<AlbumItem>
    {
        Task Add(AlbumItem album);
    }
}
