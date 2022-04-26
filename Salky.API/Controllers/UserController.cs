using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Salky.App.Dtos.Auth;
using Salky.App.Interfaces;

namespace Salky.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ITokenService tokenService;

        public UserController(IAccountService accountService,
            ITokenService tokenService)
        {
            this.accountService = accountService;
            this.tokenService = tokenService;
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await accountService.LoginAsync(userLogin);
                if (user == null) return Unauthorized("Usuário ou Senha está errado");
                else return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar Login. Erro: {ex.Message}");
            }
        }


        [HttpPost("Cadastro")]
        [AllowAnonymous]
        public async Task<IActionResult> Cadastrar(UserRegisterDto userLogin)
        {
            try
            {
                var user = await accountService.CreateAccountAsync(userLogin);
                if (user == null) return Unauthorized("Erro ao tentar criar a conta.");
                else return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar realizar o cadastro. Erro: {ex.Message}");
            }
        }

    }
}
