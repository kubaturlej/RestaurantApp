using RestaurantApp.Application.DTOs;
using RestaurantApp.Application.Pagination;
using RestaurantApp.Application.Pagination.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Application.Contracts
{
    public interface IRestaurantRepository
    {
        Task<PageResult<RestaurantDto>> GetRestaurantsAsync(RestaurantQuery query);
        Task<RestaurantDto> GetRestaurantByIdAsync(int id);
        Task<int> AddNewRestaurant(CreateRestaurantDto dto);
        Task DeleteRestaurant(int id);
        Task UpdateRestaurnat(int id, UpdateRestaurntDto dto);
    }
}
