using Salky.App.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Interfaces
{
    public interface IAccountService
    {
        Task<UserLoggedDto?> LoginAsync(UserLoginDto usrLogin);
        Task<UserLoggedDto?> CreateAccountAsync(UserRegisterDto userDto);
    }
}
