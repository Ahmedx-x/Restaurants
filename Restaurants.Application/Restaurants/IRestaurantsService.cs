using Restaurants.Application.Dtos.Restaurants;

namespace Restaurants.Application.Restaurants
{
    public interface IRestaurantsService
    {
        Task<IReadOnlyList<RestaurantDto>> GetAll();
        Task<RestaurantDto> GetById(int id);
        Task<int> Add(CreateRestaurantDto restaurant);
    }
}