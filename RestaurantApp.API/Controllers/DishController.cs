using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Application.Contracts;
using RestaurantApp.Application.DTOs;

namespace RestaurantApp.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("restaurant/{restaurantId}/dish")]
    public class DishController : ControllerBase
    {
        private readonly IDishRepository _dishRepository;

        public DishController(IDishRepository dishRepository)
        {
            _dishRepository = dishRepository;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<DishDto>> GetDishById(int id, int restaurantId)
        {
            var result = await _dishRepository.GetDishByIdAsync(id, restaurantId);

            return result;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<DishDto>>> GetDishesForRestaurant(int restaurantId)
        {
            var result = await _dishRepository.GetDishesForRestaurantAsync(restaurantId);

            return result; 
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddNewDish(int restaurantId, CreateDishDto dto)
        {
            var id = await _dishRepository.CreteNewDishForRestaurantAsync(restaurantId, dto);

            return Created("", id);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveAllDishes(int restaurantId)
        {
            await _dishRepository.RemoveDishesForRestaurantAsync(restaurantId);

            return NoContent();
        }
    }
}
