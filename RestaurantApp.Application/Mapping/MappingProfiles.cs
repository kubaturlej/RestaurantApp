using AutoMapper;
using RestaurantApp.Application.DTOs;
using RestaurantApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Application.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.ZipCode, o => o.MapFrom(s => s.Address.ZipCode))
                .ForMember(d => d.City, o => o.MapFrom(s => s.Address.City))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Address.Street));

            CreateMap<Dish, DishDto>();
            CreateMap<CreateDishDto, Dish>();
            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(d => d.Address, o => o.MapFrom(s => new Address
                {
                    City = s.City,
                    Street = s.Street,
                    ZipCode = s.ZipCode
                }));
        }
    }
}
