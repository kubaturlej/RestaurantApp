using FluentValidation.TestHelper;
using RestaurantApp.API.Validators;
using RestaurantApp.Application.Pagination.Queries;
using RestaurantApp.Domain.Entities;

namespace RestaurantApp.IntegrationTests.Validators
{
    public class RestaurantQueryValidatorTests
    {
        public static IEnumerable<object[]> GetSmapleValidData()
        {
            var list = new List<RestaurantQuery>()
            {
                new RestaurantQuery
                {
                    PageNumber = 1,
                    PageSize = 10,
                },
                new RestaurantQuery
                {
                    PageNumber = 3,
                    PageSize = 15,
                },
                new RestaurantQuery
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = nameof(Restaurant.Name)
                },
                new RestaurantQuery
                {
                    PageNumber = 10,
                    PageSize = 5,
                    SortBy = nameof(Restaurant.Category)
                }
            };


            return list.Select(rq => new object[] { rq });
        }

        public static IEnumerable<object[]> GetSmapleInvalidData()
        {
            var list = new List<RestaurantQuery>()
            {
                new RestaurantQuery
                {
                    PageNumber = 0,
                    PageSize = 5,
                },
                new RestaurantQuery
                {
                    PageNumber = 3,
                    PageSize = 12,
                },
                new RestaurantQuery
                {
                    PageNumber = 22,
                    PageSize = 5,
                    SortBy = "Test"
                },
                new RestaurantQuery
                {
                    PageNumber = 10,
                    PageSize = 55,
                    SortBy = "Test"
                }
            };


            return list.Select(rq => new object[] { rq });
        }


        [Theory]
        [MemberData(nameof(GetSmapleValidData))]
        public void Validate_ForCorrectModel_ReturnSuccess(RestaurantQuery restaurantQuery)
        {
            //arrange
            var validator = new RestaurantQueryValidator();         

            //act
            var result = validator.TestValidate(restaurantQuery);

            //assert
            result.ShouldNotHaveAnyValidationErrors();

        }

        [Theory]
        [MemberData(nameof(GetSmapleInvalidData))]
        public void Validate_ForIncorrectModel_ReturnFailure(RestaurantQuery restaurantQuery)
        {
            //arrange
            var validator = new RestaurantQueryValidator();

            //act
            var result = validator.TestValidate(restaurantQuery);

            //assert
            result.ShouldHaveAnyValidationError();

        }
    }
}
