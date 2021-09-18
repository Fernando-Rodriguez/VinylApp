using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VinylApp.Domain.DTOs.ExternalDTOs;
using VinylApp.Domain.DTOs.InternalDTOs;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate;

namespace VinylApp.Api.Services
{
    public interface IUserServices
    {
        Task<UserDTO> RetrieveUser(HttpContext context);
        Task<UserRefreshDTO> RetrieveRefresh(HttpContext context);
    }
}