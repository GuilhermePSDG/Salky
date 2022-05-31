using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.App.Dtos.Users;
using Salky.App.Interfaces;
using Salky.App.Services.User;
using static Salky.API.Controllers.Groups.GroupController;

namespace Salky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ITokenService tokenService;
        private readonly UserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(
            IAccountService accountService,
            ITokenService tokenService,
            UserService userService,
            ILogger<UserController> logger
            )
        {
            this.accountService = accountService;
            this.tokenService = tokenService;
            this.userService = userService;
            this.logger = logger;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var result = await accountService.LoginAsync(userLogin);
                if (!result.Success) return Unauthorized(result);
                else return Ok(result);
            }
            catch 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("Cadastro")]
        [AllowAnonymous]
        public async Task<IActionResult> Cadastrar(UserRegisterDto userLogin)
        {
            try
            {
                var result = await accountService.CreateAccountAsync(userLogin);
                if (!result.Success) return Unauthorized(result);
                else return Ok(result);
            }
            catch 
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> FindUserByName([FromQuery] string UserName)
        {
            var users = await this.userService.SearchUsersByName(UserName);
            return users == null ? 
                NoContent() :
                Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> FindUserByName([FromBody] UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await this.userService.UpdateUser(User.GetUserId(),userUpdateDto);
                return Ok(user);
            }
            catch
            {
                return this.StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPut("foto")]
        public async Task<IActionResult> ChangePicture(Base64 base64)
        {
            try
            {
                var result = await this.userService.ChangePictureUsingBase64(User.GetUserId(), base64.Value);
                return result == null ? BadRequest() : Ok(new { result });
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Erro ao mudar foto do usuario.");
                return BadRequest("Invalid base64");
            }
        }

    }
}
