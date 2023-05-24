using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Application.Authorization;
using RestaurantApp.Application.Contracts;
using RestaurantApp.Application.DTOs;
using RestaurantApp.Application.Exceptions;
using RestaurantApp.Application.Pagination;
using RestaurantApp.Application.Pagination.Queries;
using RestaurantApp.Domain.Entities;
using RestaurantApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Infrastructure.Repositories
{
    public class RestaurantRespository : IRestaurantRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;

        public RestaurantRespository(AppDbContext dbContext, IMapper mapper, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        public async Task<int> AddNewRestaurant(CreateRestaurantDto dto)
        {
          
            var newRestaurant = _mapper.Map<Restaurant>(dto);

            newRestaurant.CreatedById = _userContextService.GetUserId;

            _dbContext.Restaurants.Add(newRestaurant);

            await _dbContext.SaveChangesAsync();

            return newRestaurant.Id;
        }

        public async Task DeleteRestaurant(int id)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(id);

            if (restaurant == null)
                throw new NotFoundException($"Nie znaleziono restauracji o Id {id}");


            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceActionsRequirement(ResourceAction.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Restaurants.Remove(restaurant);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<RestaurantDto> GetRestaurantByIdAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(x => x.Address)
                .Include(x => x.Dishes)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (restaurant == null)
                throw new NotFoundException($"Nie znaleziono restauracji o Id {id}");

            return _mapper.Map<RestaurantDto>(restaurant);
        }

        public async Task<PageResult<RestaurantDto>> GetRestaurantsAsync(RestaurantQuery query)
        {
            var baseQuery = _dbContext
             .Restaurants
             .Include(r => r.Address)
             .Include(r => r.Dishes)
             .Where(r => query.SearchPhrase == null || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    { nameof(Restaurant.Name), r => r.Name },
                    { nameof(Restaurant.Description), r => r.Description },
                    { nameof(Restaurant.Category), r => r.Category },
                };

                var selectedColumn = columnsSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurants = await baseQuery
             .Skip(query.PageSize * (query.PageNumber - 1))
             .Take(query.PageSize)
             .ToListAsync();

            var totalItemsCount = baseQuery.Count();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            var result = new PageResult<RestaurantDto>(restaurantsDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public async Task UpdateRestaurnat(int id, UpdateRestaurntDto dto)
        {
            var restaurant = await _dbContext.Restaurants.FindAsync(id);

            if (restaurant == null)
                throw new NotFoundException($"Nie znaleziono restauracji o Id {id}");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
                new ResourceActionsRequirement(ResourceAction.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            await _dbContext.SaveChangesAsync();
        }
    }
}
