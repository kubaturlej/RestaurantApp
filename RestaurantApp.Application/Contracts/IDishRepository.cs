using RestaurantApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Application.Contracts
{
    public interface IDishRepository
    {
        Task<DishDto> GetDishByIdAsync(int id, int restaurantId);
        Task<List<DishDto>> GetDishesForRestaurantAsync(int restaurantId);
        Task<int> CreteNewDishForRestaurantAsync(int restaurantId, CreateDishDto dto);
        Task RemoveDishesForRestaurantAsync(int restaurantId);
    }
}
