using Salky.App.Dtos.Users;
using Salky.App.Interfaces;
using Salky.App.Models;
using Salky.Domain.Contracts;
using AutoMapper;

namespace Salky.App.Services.User
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepo;
        private readonly IPasswordMannager signProvider;
        private readonly IDispatcher dispatcher;

        public AccountService(
            IMapper mapper,
            ITokenService tokenService,
            IUserRepository userRepo,
            IPasswordMannager signProvider,
            IDispatcher dispatcher
            )
        {
            _mapper = mapper;

            this.tokenService = tokenService;
            this.userRepo = userRepo;
            this.signProvider = signProvider;
            this.dispatcher = dispatcher;
        }

        public async Task<Response<UserLoggedDto?>> LoginAsync(UserLoginDto usrLogin)
        {
            var user = (await userRepo.GetUserByName(usrLogin.UserName));
            if (user == null) return "Usuário não encontrado.";
            var Succeeded = await signProvider.CheckPasswordSignInAsync(user.PassWordHash, usrLogin.Password);
            if (Succeeded)
            {
                var usrRetorno = this._mapper.Map<UserLoggedDto>(user);
                var token = await this.tokenService.CreateToken(user);
                usrRetorno.Token = token.value;
                usrRetorno.TokenExpire = token.expires;
                return new Response<UserLoggedDto?>(usrRetorno);
            }
            return "Usuário ou senha Inválidos.";
        }
        public async Task<Response<UserLoggedDto?>> CreateAccountAsync(UserRegisterDto userRegisterDto)
        {
            if(userRegisterDto.Password.Length < 6) return "Senha muito curta.";
            var found = await userRepo.GetUserByName(userRegisterDto.UserName);
            if (found != null) return "Usuário já existe.";
            var hash = await signProvider.CreateHashAsync(userRegisterDto.Password);
            Domain.Models.UserModels.User user = new (userRegisterDto.UserName, hash);
            if(!user.IsValid(out var msg)) return msg;
            userRepo.Add(user);
            await userRepo.EnsureSaveChangesAsync();
            user = await userRepo.GetById(user.Id, false) ?? throw new Exception("??");
            var usrRetorno = this._mapper.Map<UserLoggedDto>(user);
            var token = await this.tokenService.CreateToken(user);
            usrRetorno.Token = token.value;
            usrRetorno.TokenExpire = token.expires;
            return new Response<UserLoggedDto?>(usrRetorno);
        }

    }
}
