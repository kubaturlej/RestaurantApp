using FluentValidation;
using RestaurantApp.Application.DTOs;
using RestaurantApp.Infrastructure.Persistence;

namespace RestaurantApp.API.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator(AppDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .MinimumLength(4);

            RuleFor(x => x.ConfirmPassword)
                .Equal(y => y.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var email = dbContext.Users.Any(x => x.Email == value);
                    if (email)
                    {
                        context.AddFailure("Email", "This emails is taken.");
                    }
                });
        }
    }
}
