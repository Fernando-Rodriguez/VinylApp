using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylApp.Api.Services;
using VinylApp.Domain.DTOs.ExternalDTOs;
using VinylApp.Domain.Models.VinylAppModels.UserAggregate;
using VinylApp.Domain.Services;
using VinylApp.Infrastructure.UnitOfWork;

namespace VinylApp.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserServices _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(
            ILogger<UserController> logger,
            IUnitOfWork unitOfWork,
            IUserServices userService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersById()
        {
            try
            {
                var myNewResult = await _userService.RetrieveUser(HttpContext);
                return Ok(myNewResult);
            }
            catch (Exception err)
            {
                _logger.LogError($"Error getting user: {err}");
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost("new")]
        public async Task<IActionResult> CreateUser([FromBody] NewUserDto loginUser)
        {
            loginUser.UserPass = EncryptionService.Hash(loginUser.UserPass);
            try
            {
                var user = await _unitOfWork
                    .Users
                    .Find(user =>
                        user.UserAuthorization.UserName == loginUser.UserName &&
                        user.UserAuthorization.UserPass == loginUser.UserPass
                    );

                if (user.Count > 0) return BadRequest();
                
                var newUser = new User(
                    loginUser.UserName, 
                    loginUser.UserName, 
                    loginUser.UserPass, 
                    loginUser.UserInfo);
                
                await _unitOfWork.Users.Add(newUser);
                await _unitOfWork.SaveChanges();
                
                return Ok(new
                {
                    newUser.Id,
                    newUser.ScreenName
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in creating new user: {ex}");
                return BadRequest();
            }
        }

        [HttpPost("update/info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoDto dto)
        {
            try
            {
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                userFromDb.UpdateUserInfo(dto.NewUserInfo);
                await _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(500);
            }
        }

        [HttpPost("update/name")]
        public async Task<IActionResult> UpdateScreenName([FromBody] UpdateUserNameDto newUserName)
        {
            try
            {
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                userFromDb.UpdateScreenName(newUserName.NewUserName);
                await _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(500);
            }
        }
        
        [HttpPost("update/login")]
        public async Task<IActionResult> UpdateLoginName([FromBody] UpdateUserNameDto newUserName)
        {
            try
            {
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                userFromDb.UpdateUserName(newUserName.NewUserName);
                await _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(500);
            }
        }
        
        [HttpPost("update/pass")]
        public async Task<IActionResult> UpdateLoginPass([FromBody] UpdateUserPassDto newUserName)
        {
            try
            {
                var hashPass = EncryptionService.Hash(newUserName.NewUserPass);
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                userFromDb.UpdateUserPass(hashPass);
                await _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                _unitOfWork.Users.Remove(userFromDb);
                await _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return StatusCode(500);
            }
        }
    }
}