using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<IReadOnlyList<Restaurant>> GetAllAsync();
        Task<Restaurant?> GetByIdAsync(int id);
        Task<int> AddAsync(Restaurant restaurant);
        Task DeleteByIdAsync(int id);
        Task DeleteAsync(Restaurant restaurant);
        Task SaveChanges();
    }
}
