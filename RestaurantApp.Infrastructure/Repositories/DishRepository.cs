using AutoMapper;
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

namespace RestaurantApp.Infrastructure.Repositories
{
    public class DishRepository : IDishRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> CreteNewDishForRestaurantAsync(int restaurantId, CreateDishDto dto)
        {
        
            var dish = _mapper.Map<Dish>(dto);

            dish.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dish);
            await _dbContext.SaveChangesAsync();

            return dish.Id;
        }

        public async Task<DishDto> GetDishByIdAsync(int id, int restaurantId)
        {
            var restaurant = await GetRestaurant(restaurantId);

            var dish = restaurant.Dishes.FirstOrDefault(x => x.Id == id);

            if (dish == null)
                throw new NotFoundException($"Nie znaleziono dania o Id {id}");

            return _mapper.Map<DishDto>(dish);
        }

        public async Task<List<DishDto>> GetDishesForRestaurantAsync(int restaurantId)
        {
            var restaurant = await GetRestaurant(restaurantId);

            return _mapper.Map<List<DishDto>>(restaurant.Dishes);
        }


        public async Task RemoveDishesForRestaurantAsync(int restaurantId)
        {
            var restaurant = await GetRestaurant(restaurantId);
     
            _dbContext.RemoveRange(restaurant.Dishes);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<Restaurant> GetRestaurant(int restaurantId)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(x => x.Dishes)
                .FirstOrDefaultAsync(x => x.Id == restaurantId);

            if (restaurant == null)
                throw new NotFoundException($"Nie znaleziono restauracji o Id {restaurantId}");
            return restaurant;
        }
    }
}
