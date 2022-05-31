using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.App.Services.Friends;

namespace Salky.API.Controllers.Friends
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendController : ControllerBase
    {
        private readonly FriendService friendService;

        public FriendController(FriendService friendService)
        {
            this.friendService = friendService;
        }
        [HttpPost("add/{userId}")]
        public async Task<IActionResult> AddUser(Guid userId)
        {
            try
            {
                var friend = await friendService.SendFriendRequest(User.GetUserId(), userId);
                return friend == null ? BadRequest() : Ok(friend);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("accept/{friendId}")]
        public async Task<IActionResult> AcceptFriendRequest(Guid friendId)
        {
            try
            {
                var res = await friendService.AcceptFriend(User.GetUserId(), friendId);
                return res != null ? Ok(res) : BadRequest(res);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var friends = await friendService.GetAll(User.GetUserId());
                return friends == null ? BadRequest() : Ok(friends);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }





    }
}
