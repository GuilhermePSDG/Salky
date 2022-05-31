using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.App.Dtos.Group;
using Salky.App.Services.Group;

namespace Salky.API.Controllers.Groups
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public partial class GroupController : ControllerBase
    {
        private readonly GroupService groupService;

        public GroupController(GroupService groupService)
        {
            this.groupService = groupService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await this.groupService.GetAllGroupsOfUser(User.GetUserId());
            return Ok(groups);
        }


        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid groupId)
        {
            var group = await this.groupService.GetById(User.GetUserId(), groupId);
            if (group == null) return BadRequest();
            else return Ok(group);
        }
        [HttpPut("picture/{GroupId}")]
        public async Task<IActionResult> ChangeGroupPicture([FromRoute]Guid GroupId,[FromBody] Base64 Base64)
        {
            try
            {
                var result = await this.groupService.ChangeGroupPictureUsingBase64(User.GetUserId(), GroupId, Base64.Value);
                return result == null ? BadRequest() : Ok(result);
            }
            catch
            {
                return BadRequest("Invalid base64");
            }
        }

        [HttpPost("create/{GroupName}")]
        public async Task<IActionResult> CreateNewGroup([FromRoute] string GroupName)
        {
            try
            {
                var group = await groupService.CreateNewPublicGroup(User.GetUserId(), GroupName);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{groupid}")]
        public async Task<IActionResult> DeleteGroup([FromRoute] Guid groupid)
        {
            try
            {
                var removed = await groupService.RemoveGroup(User.GetUserId(), groupid);
                return removed ? Ok(removed) : BadRequest("Não foi possivel remover o grupo");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
