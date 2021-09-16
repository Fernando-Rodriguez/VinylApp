using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VinylApp.Api.Services;
using VinylApp.Domain.DTOs.ExternalDTOs;
using VinylApp.Domain.Models.VinylAppModels.GroupAggregate;
using VinylApp.Infrastructure.UnitOfWork;

namespace VinylApp.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GroupController> _logger;
        private readonly IUserServices _userService;

        public GroupController(
            ILogger<GroupController> logger, 
            IUnitOfWork unitOfWork,
            IUserServices userService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyGroup()
        {
            var user = await _userService.RetrieveUser(HttpContext);
            var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
            var groups = userFromDb.GetMyGroups();
            return Ok( new
            {
                myGroups = groups
            });
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateNewGroup([FromBody] GroupDto groupDto)
        {
            try
            {
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                
                var group = new Group(groupDto.GroupName);
                
                group.AddMembers(userFromDb);
                
                if (groupDto.GroupMemberIds.Count > 0)
                {
                    foreach (var id in groupDto.GroupMemberIds)
                    {
                        var u = await _unitOfWork.Users.GetById(int.Parse(id));
                        group.AddMembers(u);
                    }
                }
                
                userFromDb.AddGroup(group);
                await _unitOfWork.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(500);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMemberToGroup([FromBody] AddMemberToGroupDto dto)
        {
            try
            {
                var group = await _unitOfWork.Group.GetById(dto.GroupId);
                foreach (var id in dto.NewMembers)
                {
                    var u = await _unitOfWork.Users.GetById(int.Parse(id));
                    group.AddMembers(u);
                }
                await _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception err)
            {
                _logger.LogError(err.ToString());
                return BadRequest();
            }
        }

        [HttpPost("update/name")]
        public async Task<IActionResult> UpdateGroupName([FromBody] UpdateGroupNameDto dto)
        {
            try
            {
                var user = await _userService.RetrieveUser(HttpContext);
                var userFromDb = await _unitOfWork.Users.GetById(int.Parse(user.Id));
                var userGroup = userFromDb.GetMyGroupbyId(dto.Id);
                userGroup.UpdateGroupName(dto.Name);
                await _unitOfWork.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return BadRequest();
            }
        }
    }
}
