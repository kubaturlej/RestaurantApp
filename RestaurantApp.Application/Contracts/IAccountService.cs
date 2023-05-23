using RestaurantApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Application.Contracts
{
    public interface IAccountService
    {
        Task RegisterUser(RegisterDto dto);
        Task<UserDto> LoginUser(LoginDto dto);
    }
}
