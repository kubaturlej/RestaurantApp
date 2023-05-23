using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.API.Validators;
using RestaurantApp.Application.DTOs;
using RestaurantApp.Application.Pagination.Queries;
using RestaurantApp.Domain.Entities;
using RestaurantApp.Infrastructure.Persistence;

namespace RestaurantApp.IntegrationTests.Validators
{
    public class RestaurantDtoValidatorTests
    {
        private readonly AppDbContext _dbContext;

        public RestaurantDtoValidatorTests()
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseInMemoryDatabase("TestDb");

            _dbContext = new AppDbContext(builder.Options);

            Seed();
        }

        private void Seed()
        {
            var testUsers = new List<User>()
            {
                new User
                {
                    Email = "test2@test.pl",
                    FirstName = "Test",
                    LastName = "Test",
                    Nationality = "Test",
                    PasswordHash = "12345"
                },
                new User
                {
                     Email = "test3@test.pl",
                     FirstName = "Test",
                     LastName = "Test",
                     Nationality = "Test",
                     PasswordHash = "12345"
                }
            };
            _dbContext.Users.AddRange(testUsers);
            _dbContext.SaveChanges();
        }

        [Fact]
        public void Validate_ForValidModel_ReturnSuccess() 
        {
            //arrange

            var model = new RegisterDto
            {
                Email = "test@test.pl",
                FirstName = "Test",
                LastName = "Test",
                Password = "qwe12",
                ConfirmPassword = "qwe12",
                Nationality = "Poland",
                DateOfBirth = new DateTime(1999, 9, 12)
            };

      
            var validator = new RegisterDtoValidator(_dbContext);

            //act
            var result = validator.TestValidate(model);

            //assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_ForInValidModel_ReturnFailure()
        {
            //arrange

            var model = new RegisterDto
            {
                Email = "test2@test.pl",
                FirstName = "Test",
                LastName = "Test",
                Password = "qwe12",
                ConfirmPassword = "qwe12",
                Nationality = "Poland",
                DateOfBirth = new DateTime(1999, 9, 12)
            };


            var validator = new RegisterDtoValidator(_dbContext);

            //act
            var result = validator.TestValidate(model);

            //assert
            result.ShouldHaveAnyValidationError();
        }
    }
}
