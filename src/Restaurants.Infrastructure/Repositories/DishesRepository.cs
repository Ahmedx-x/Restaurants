using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories
{
    public class DishesRepository(RestaurantsDbContext dbContext)
        : IDishesRepository
    {
        public async Task<int> AddAsync(Dish dish)
        {
            dbContext.Dishes.Add(dish);
            await dbContext.SaveChangesAsync();
            return dish.Id;
        }

        public async Task Delete(IEnumerable<Dish> dishes)
        {
            dbContext.Dishes.RemoveRange(dishes);
            await dbContext.SaveChangesAsync();
        }
    }
}
