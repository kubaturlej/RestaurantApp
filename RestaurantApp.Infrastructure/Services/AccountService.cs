using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Application.Contracts;
using RestaurantApp.Application.DTOs;
using RestaurantApp.Application.Exceptions;
using RestaurantApp.Domain.Entities;
using RestaurantApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;

        public AccountService(AppDbContext dbContext, IPasswordHasher<User> passwordHasher, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<UserDto> LoginUser(LoginDto dto)
        {
            var user  = await _dbContext.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new NotFoundException("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new NotFoundException("Invalid credentials");

            var token = _tokenService.GetToken(user);

            return new UserDto
            {
                Email = dto.Email,
                Token = token,
            };
        }

        public async Task RegisterUser(RegisterDto dto)
        {

            var newUser = new User()
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;
            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}
