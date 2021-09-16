using System;
using System.Threading.Tasks;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;
using VinylApp.Domain.Models.VinylAppModels.GroupAggregate;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate;

namespace VinylApp.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAlbumRepo Albums { get; }
        IUserRepo Users { get; }
        IGroupRepo Group { get; }
        Task SaveChanges();
    }
}