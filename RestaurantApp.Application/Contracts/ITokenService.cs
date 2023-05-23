using RestaurantApp.Domain.Entities;

namespace RestaurantApp.Application.Contracts
{
    public interface ITokenService
    {
        string GetToken(User user);
    }
}
