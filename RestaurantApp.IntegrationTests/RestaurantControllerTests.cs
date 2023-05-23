using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using RestaurantApp.Application.DTOs;
using RestaurantApp.Application.Pagination.Queries;
using RestaurantApp.Domain.Entities;
using RestaurantApp.Infrastructure.Persistence;
using RestaurantApp.IntegrationTests.Helpers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RestaurantApp.IntegrationTests
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Program> _factory;

        public RestaurantControllerTests(WebApplicationFactory<Program> factory)
        {

            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<AppDbContext>));
                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                        services.AddMvc(opt => opt.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("RestaurantDb"));
                    });
                });
             _httpClient = _factory.CreateClient();
        }


        [Theory]
        [InlineData("/restaurant")]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult(string route)
        {
            //arragne

            var jsonBody = "{\"searchPhrase\": \"\",\"sortBy\": \"Name\", \"pageNumber\": 1, \"pageSize\": 30, \"sortDirection\": 0}";

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            //act

            var response = await _httpClient.PostAsync(route, content);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateRestaurant_WithValidModel_ReturnCreatedResult()
        {
            //arrange
            var model = new CreateRestaurantDto
            {
                Name = "Test Restaurant",
                City = "Toronto",
                Street = "Słoneczna 10"
            };

            var content = model.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/restaurant/create", content);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateRestaurant_WithInvalidModel_ReturnBadRequestResult()
        {
            //arange 
            var model = new CreateRestaurantDto
            {
                Name = "Test Restaurant",
                City = "Toronto",
                Street = "Słoneczna 10"
            };

            var content = model.ToJsonHttpContent();

            //act
            var response = await _httpClient.PostAsync("/restaurant/create", content);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        private void SeedRestaurant(Restaurant restaurant)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

            dbContext.Restaurants.Add(restaurant);
            dbContext.SaveChanges();
        }

        [Fact]
        public async Task DeleteRestaurant_ForRestaurantOwner_ReturnNoContentResult()
        {
            //arange 
            var restaurant = new Restaurant
            {
                CreatedById = 1,
                Name = "Test",
            };

            SeedRestaurant(restaurant);

            //act
            var response = await _httpClient.DeleteAsync($"/restaurant/{restaurant.Id}");

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteRestaurant_ForNonRestaurantOwner_ReturnForbiddenResult()
        {
            //arange 
            var restaurant = new Restaurant
            {
                CreatedById = 800,
                Name = "Test",
            };

            SeedRestaurant(restaurant);

            //act
            var response = await _httpClient.DeleteAsync($"/restaurant/{restaurant.Id}");

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task DeleteRestaurant_ForNonExistingRestaurant_ReturnNotFoundResult()
        {
            //arange 


            //act
            var response = await _httpClient.DeleteAsync($"/restaurant/{123123}");

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
