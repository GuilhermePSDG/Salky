using Salky.Persistence.Persist;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Salky.App.Interfaces;
using Salky.Domain;
using Salky.App.Dtos.Auth;
using Salky.App.Dtos.Users;

namespace Salky.App.Services
{
    public class UserService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService tokenService;
        private readonly UserRepository userRepo;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper,
            ITokenService tokenService,
            UserRepository userRepo
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
     
            this.tokenService = tokenService;
            this.userRepo = userRepo;
        }

        
        public async Task<UserLoggedDto?> LoginAsync(UserLoginDto usrLogin)
        {
            try
            {
                var user = (await _userManager.Users.SingleOrDefaultAsync(user => user.UserName == usrLogin.UserName));
                user.ThrowIfNull("Usuario não encontrado");
                var result =  await _signInManager.CheckPasswordSignInAsync(user,usrLogin.Password , false);
                if(result != null && result.Succeeded)
                {
                    var usrRetorno = this._mapper.Map<UserLoggedDto>(user);
                    usrRetorno.Token = await this.tokenService.CreateToken(user);
                    return usrRetorno;
                }
                throw new Exception("Usuario ou senha invalidos.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao efetuar login.\n{ex.Message}");
            }
        }
        public async Task<UserLoggedDto?> CreateAccountAsync(UserRegisterDto userRegisterDto)
        {
            try
            {
                var found = await _userManager.FindByNameAsync(userRegisterDto.UserName);
                if(found != null) throw new Exception("Usuario já existe");

                var user = _mapper.Map<User>(userRegisterDto);
                var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

                if (result.Succeeded)
                {
                    var usrRetorno = this._mapper.Map<UserLoggedDto>(user);
                    usrRetorno.Token = await this.tokenService.CreateToken(user);
                    return usrRetorno;
                }
                else
                {
                    var erros = new String(result.Errors.Select(x => x.Description + ",").SelectMany(f => f).ToArray()).Trim(',');
                    throw new Exception("Não foi possivel criar a conta.\nMotivos : " + erros);
                }

                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Erro ao tentar Criar Usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserDto> GetUserById(Guid userId)
        {
            var user = await this.userRepo.GetById(userId, true);
            return this._mapper.Map<UserDto>(user.ThrowIfNull("Usuario não encontrado"));
        }
        

        public async Task<bool> DeleteUser(Guid userId)
        {
            var user = await this.userRepo.GetById(userId, false);
            this.userRepo.Delete(user.ThrowIfNull("Usuario não encontrado"));
            return (await this.userRepo.SaveChangesAsync()) > 0;
        }

    }
    
}
