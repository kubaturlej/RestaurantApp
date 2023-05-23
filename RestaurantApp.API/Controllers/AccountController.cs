using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Application.Contracts;
using RestaurantApp.Application.DTOs;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDto dto)
        {
            await _accountService.RegisterUser(dto);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDto dto)
        {
            var userDto = await _accountService.LoginUser(dto);

            return Ok(userDto);
        }
    }
}
