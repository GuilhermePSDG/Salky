using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.App.Dtos.Group;
using Salky.App.Services.Group;

namespace Salky.API.Controllers.Groups
{
    [Route("api/group/member")]
    [ApiController]
    [Authorize]
    public class GroupMemberController : ControllerBase
    {
        private readonly GroupService groupService;
        private readonly GroupMemberService groupMemberService;

        public GroupMemberController(GroupService groupService,GroupMemberService groupMemberService)
        {
            this.groupService = groupService;
            this.groupMemberService = groupMemberService;
        }

        //[HttpPost("{userid}/{groupid}")]
        //public async Task<IActionResult> AddUserInGroup([FromRoute] Guid userid, [FromRoute] Guid groupid)
        //{
        //    try
        //    {
        //        var group = await groupService.AddUserInGroup(userid, groupid);
        //        return Ok(group);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("{groupid}")]
        public async Task<IActionResult> GetAllMembers([FromRoute] Guid groupid)
        {
            List<GroupMemberDto>? members = await groupMemberService.GetMembersOfGroup(User.GetUserId(), groupid);
            return Ok(members);
        }

        [HttpGet("self/{groupid}")]
        public async Task<IActionResult> GetMember([FromRoute] Guid groupid)
        {
            var member = await this.groupMemberService.GetMemberWithRole(User.GetUserId(), groupid);
            return member != null ? Ok(member) : BadRequest();
        }

        [HttpDelete("{groupid}/{userId}")]
        public async Task<IActionResult> DeleteUserOfGroup([FromRoute] Guid groupid, [FromRoute] Guid userid)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }



    }
}
