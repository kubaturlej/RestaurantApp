using FluentValidation;
using RestaurantApp.Application.Pagination.Queries;
using RestaurantApp.Domain.Entities;

namespace RestaurantApp.API.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private int[] allowedPageSize = new [] { 5, 10, 15 };

        private string[] allowedSortByColumnNames =
            new string[] { nameof(Restaurant.Name), nameof(Restaurant.Category), nameof(Restaurant.Description) };

        public RestaurantQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSize.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in {string.Join(", ", allowedPageSize)}");
                }
            });

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional or must be in {string.Join(", ", allowedSortByColumnNames)}");
        }
    }
}
