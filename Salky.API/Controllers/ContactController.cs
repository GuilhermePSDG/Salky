using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.App.Services;
using System.Security.Claims;

namespace Salky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly ContactService contactService;

        public ContactController(ContactService contactService)
        {
            this.contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            try
            {
                var usrId = User.GetUserId();
                var usrContacs = await contactService.GetAllAsync(usrId);
                return Ok(usrContacs);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchContact([FromQuery] string contactName)
        {
            try
            {
                var usrId = User.GetUserId();
                var contact = await contactService.GetUserByName(contactName);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddContact([FromQuery] string contactName)
        {
            try
            {
                var usrId = User.GetUserId();
                var contact = await this.contactService.AddContactByUserNameAsync(usrId, contactName);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



    }
}
