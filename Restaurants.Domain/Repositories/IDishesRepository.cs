using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IDishesRepository
    {
        Task<int> AddAsync(Dish dish);
        Task Delete(IEnumerable<Dish> dishes);
    }
}
