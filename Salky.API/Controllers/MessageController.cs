using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.API.Models;
using Salky.App.Services;

namespace Salky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly MessageService messageService;

        public MessageController(MessageService messageService)
        {
            this.messageService = messageService;
        }
        //[HttpPost]
        //public async Task<IActionResult> AddMessage([FromBody] AddMessage msg)
        //{
        //    try
        //    {
        //        var addded = await this.messageService.AdicionarMensagemParaAmbos(User.GetUserId(), msg.contactId, msg.content);
        //        if(addded)
        //            return Ok();
        //        else
        //            throw new Exception();
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest($"Não foi possivel adicionar a mensagem.\n{ex.Message}"); 
        //    }
        //}
        [HttpGet("{contactid}")]
        public async Task<IActionResult> GetMessages([FromRoute] Guid contactid)
        {
            try
            {
                var msgs = await this.messageService.GetMessagesByContactId(User.GetUserId(),contactid);
                return Ok(msgs);
            }
            catch (Exception ex)
            {
                return BadRequest($"Não foi possivel obter a mensagem.\n{ex.Message}");
            }
        }
    }
}
