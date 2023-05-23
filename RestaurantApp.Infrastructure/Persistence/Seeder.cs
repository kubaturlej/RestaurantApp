using Bogus;
using RestaurantApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Infrastructure.Persistence
{
    public static class Seeder
    {
        public static void SeedData(AppDbContext dbContext)
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                dbContext.SaveChanges();
            }
        }

        private static IEnumerable<Restaurant> GetRestaurants()
        {
            var addressGenerator = new Faker<Address>()
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.ZipCode, f => f.Address.ZipCode())
                .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                .RuleFor(a => a.Country, f => f.Address.Country());


            var dishesGenerator = new Faker<Dish>()
                .RuleFor(d => d.Name, f => f.Commerce.ProductName())
                .RuleFor(d => d.Description, f => f.Lorem.Sentence())
                .RuleFor(d => d.Price, f => f.Random.Decimal(1, 50))
                .RuleFor(i => i.ImageUrl, f => f.Image.PicsumUrl());


            var restaurantGenerator = new Faker<Restaurant>()
                .RuleFor(r => r.Name, f => f.Company.CompanyName())
                .RuleFor(r => r.Description, f => f.Lorem.Paragraph())
                .RuleFor(r => r.Category, f => f.Random.ArrayElement(new[] { "Italian", "Mexican", "Japanese", "Indian", "American", "Chinese" }))
                .RuleFor(r => r.HasDelivery, f => f.Random.Bool())
                .RuleFor(i => i.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(r => r.ContactEmail, f => f.Internet.Email())
                .RuleFor(r => r.ContactNumber, f => f.Phone.PhoneNumber())
                .RuleFor(r => r.Dishes, f => dishesGenerator.Generate(f.Random.Int(1, 10)))
                .RuleFor(r => r.Address, f => addressGenerator.Generate())
                .RuleFor(d => d.CreatedDate, f => f.Date.Between(new DateTime(2000, 1, 1), DateTime.Today))
                .RuleFor(d => d.CreatedById, f => f.Random.Int(9,9));

            var restaurants = restaurantGenerator.Generate(100);


            return restaurants;
        }


    }
}
