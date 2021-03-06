using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylApp.Api.Services;
using VinylApp.Infrastructure.UnitOfWork;
using System.Linq;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;
using VinylApp.Domain.DTOs.ExternalDTOs;
using Microsoft.AspNetCore.Authorization;
using System;
using VinylApp.Domain.DTOs.InternalDTOs;

namespace VinylApp.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly ILogger<AlbumController> _logger;
        private readonly IUserServices _userService;
        private readonly IUnitOfWork _unitOfWork;

        public AlbumController(
            ILogger<AlbumController> logger,
            IUserServices userService,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAlbums()
        {
            try
            {
                var allAlbums = await _unitOfWork.Albums.GetAll();

                return Ok(new MultipleItemResponseDto
                {
                    Root = allAlbums.Select(x => new SingleItemResponseDto
                    {
                        Item = new AlbumDTO
                        {
                            Id = x.Id,
                            AlbumName = x.AlbumName,
                            ArtistName = x.ArtistName,
                            AlbumArtworkUrl = x.AlbumArtworkUrl
                        }
                    })
                });
            }
            catch (Exception err)
            {
                _logger.LogError("Issue generating albums.");
                _logger.LogError(err.ToString());
                return Problem();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var album = await _unitOfWork.Albums.GetById(id);
                return Ok(new SingleItemResponseDto
                {
                    Item = new AlbumDTO
                    {
                        Id = album.Id,
                        AlbumName = album.AlbumName,
                        ArtistName = album.ArtistName,
                        AlbumArtworkUrl = album.AlbumArtworkUrl
                    }
                });
            }
            catch (Exception err)
            {
                _logger.LogError("Issue generating albums.");
                _logger.LogError(err.ToString());
                return Problem();
            }
        }

        [HttpGet("me/basic")]
        public async Task<IActionResult> GetMyAlbumsBasic()
        {
            try
            {
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                var albums = userFromDb.GetCoreAlbumInfo();
                return Ok(new MultipleItemResponseDto
                {
                    Root = albums.Select(x => new SingleItemResponseDto
                    {
                        Item = new AlbumDTO
                        {
                            Id = x.Id,
                            AlbumName = x.AlbumName,
                            ArtistName = x.ArtistName,
                            AlbumArtworkUrl = x.AlbumArtworkUrl
                        }
                    })
                });
            }
            catch (Exception err)
            {
                _logger.LogError("Issue generating albums.");
                _logger.LogError(err.ToString());
                return Problem();
            }
        }
        
        [HttpGet("me/full")]
        public async Task<IActionResult> GetMyAlbumsFull()
        {
            try
            {
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                var albums = userFromDb.GetFullAlbums();
                return Ok(new MultipleItemResponseDto
                {
                    Root = albums.Select(x => new SingleItemResponseDto
                    {
                        Item = new OwnedAlbumDTO
                        {
                            Id = x.Id,
                            Album= new AlbumDTO
                            {
                                Id = x.AlbumItem.Id,
                                ArtistName = x.AlbumItem.ArtistName,
                                AlbumName = x.AlbumItem.AlbumName,
                                AlbumArtworkUrl = x.AlbumItem.AlbumArtworkUrl
                            },
                            AlbumInfo = x.AlbumInfo,
                            UserId = x.UserId
                        }
                    })
                });
            }
            catch (Exception err)
            {
                _logger.LogError("Issue generating albums.");
                _logger.LogError(err.ToString());
                return Problem();
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> PostAlbum([FromBody] NewAlbumDTO albumDto)
        {
            try
            {
                var album = new AlbumItem
                {
                    AlbumName = albumDto.AlbumName,
                    ArtistName = albumDto.ArtistName
                };

                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                if (CheckIfUserOwnsAlbum()) return BadRequest("User already owns album");

                var albumInDb = await _unitOfWork
                    .Albums
                    .Find(a => 
                        a.ArtistName == album.ArtistName && 
                        a.AlbumName == album.AlbumName);

                if (albumInDb.Count == 0)
                {
                    await SetAlbumIfAlbumDoesNotExist();
                }
                else
                {
                    userFromDb.AddAlbum(albumInDb.First());
                }

                await _unitOfWork.SaveChanges();
                return Ok();

                async Task SetAlbumIfAlbumDoesNotExist()
                {
                    await _unitOfWork.Albums.Add(album);
                    await _unitOfWork.SaveChanges();
                    var albumFromDb = await _unitOfWork.Albums.Find(a => a == album);
                    userFromDb.AddAlbum(albumFromDb.First());
                }
                
                bool CheckIfUserOwnsAlbum()
                {
                    var userOwnedAlbum = userFromDb.Albums.Find(a =>
                        a.AlbumItem.AlbumName == album.AlbumName && 
                        a.AlbumItem.ArtistName == album.ArtistName);
                    return userOwnedAlbum != null;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest();
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            try
            {
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                var album = userFromDb.FindAlbumWithId(id);
                if (album == null) return BadRequest();
                userFromDb.RemoveAlbum(album.Id);
                await _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return BadRequest();
            }
        }
    }
}
