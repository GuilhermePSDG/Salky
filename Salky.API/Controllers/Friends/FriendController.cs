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
