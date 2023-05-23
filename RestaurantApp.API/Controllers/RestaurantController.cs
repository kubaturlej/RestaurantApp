using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Application.Contracts;
using RestaurantApp.Application.DTOs;
using RestaurantApp.Application.Pagination;
using RestaurantApp.Application.Pagination.Queries;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantDto>> GetRestaurantById(int id)
        {
            return await _restaurantRepository.GetRestaurantByIdAsync(id);
        }

        [Authorize(Policy = "RequireManagerRole")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<PageResult<RestaurantDto>>> GetRestaurants(RestaurantQuery query)
        {
            return await _restaurantRepository.GetRestaurantsAsync(query);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRestaurant(CreateRestaurantDto dto)
        {
            int restaurantId = await _restaurantRepository.AddNewRestaurant(dto);
            return Created($"restaurant/{restaurantId}", null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, UpdateRestaurntDto dto)
        {
            await _restaurantRepository.UpdateRestaurnat(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int restaurantId)
        {
            await _restaurantRepository.DeleteRestaurant(restaurantId);
            return NoContent();
        }

    }
}
