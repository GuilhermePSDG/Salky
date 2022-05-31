using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.API.Models;
using Salky.App.Services.Group;

namespace Salky.API.Controllers.Groups
{
    [Route("api/group/message")]
    [ApiController]
    [Authorize]
    public class GroupMessageController : ControllerBase
    {
        private readonly GroupMessageService messageService;

        public GroupMessageController(GroupMessageService messageService)
        {
            this.messageService = messageService;
        }
        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] AddMessage msg)
        {
            try
            {
                var addded = await messageService.AddMessage(User.GetUserId(), msg.GroupId, msg.Content);
                if (addded != null)
                    return Ok(addded);
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                return BadRequest($"Não foi possivel adicionar a mensagem.\n{ex.Message}");
            }
        }
        [HttpPost("img")]
        public async Task<IActionResult> AddMessageImage(string Base64)
        {
            throw new NotImplementedException();

        }
        public record MessageGet(Guid groupId,int currentPage,int pageSize);
        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] MessageGet messageGet)
        {
            try
            {
                messageGet.Deconstruct(out var groupId,out var currentPage,out var pageSize);
                var msgs = await messageService.GetMessagesOfGroup(User.GetUserId(), groupId,currentPage,pageSize);
                return Ok(msgs);
            }
            catch (Exception ex)
            {
                return BadRequest($"Não foi possivel obter a mensagem.\n{ex.Message}");
            }
        }
        [HttpDelete("{MessageId}")]
        public async Task<IActionResult> DeleteMessageAsync([FromRoute] Guid MessageId)
        {
            try
            {
                var removed = await messageService.RemoveMessage(User.GetUserId(), MessageId);
                return removed != null ? Ok(removed) : BadRequest(removed);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Não foi remover a mensagem.\n{ex.Message}");
            }
        }
    }
}
