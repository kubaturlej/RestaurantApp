using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RestaurantApp.Application.Contracts;
using RestaurantApp.Application.DTOs;
using RestaurantApp.Infrastructure.Persistence;
using RestaurantApp.IntegrationTests.Helpers;
using System.Net.Http;

namespace RestaurantApp.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();

        public AccountControllerTests(WebApplicationFactory<Program> factory)
        {

            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<AppDbContext>));
                        services.Remove(dbContextOptions);

                        services.AddSingleton<IAccountService>(_accountServiceMock.Object);

                        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("RestaurantDb"));
                    });
                });
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOkResult()
        {
            //arrange

            var registerUser = new RegisterDto
            {
                Email = "test@wp.pl",
                FirstName = "Test",
                LastName = "Test",
                Password = "password12",
                ConfirmPassword = "password12",
                Nationality = "Poland",
                DateOfBirth = new DateTime(1999, 9, 12)
            };

            var content = registerUser.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/account/register", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.Headers.Should().NotBeNull();
        }


        [Fact]
        public async Task RegisterUser_ForInvalidModel_ReturnsBadRequestResult()
        {
            //arrange

            var registerUser = new RegisterDto
            {
                Email = "test",
                FirstName = "Test",
                LastName = "Test",
                Password = "qwe",
                ConfirmPassword = "qwe",
                Nationality = "Poland",
                DateOfBirth = new DateTime(1999, 9, 12)
            };

            var content = registerUser.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/account/register", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            response.Headers.Should().NotBeNull();
        }

        [Fact]
        public async Task Login_ForRegisteredUser_ReturnsOkResult()
        {
            //arrange
            _accountServiceMock
                .Setup(e => e.LoginUser(It.IsAny<LoginDto>()))
                .Returns(Task.FromResult(new UserDto { Email="test", Token="jwt"}));
    

            var loginDto = new LoginDto { Email = "test", Password = "test" };
            var content = loginDto.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/account/login", content);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }

}
