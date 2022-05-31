using Salky.App.Dtos.Users;
using Salky.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salky.App.Interfaces
{
    public interface IAccountService
    {
        Task<Response<UserLoggedDto?>> LoginAsync(UserLoginDto usrLogin);
        Task<Response<UserLoggedDto?>> CreateAccountAsync(UserRegisterDto userDto);
    }
}
