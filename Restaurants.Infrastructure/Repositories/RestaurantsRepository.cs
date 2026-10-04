using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories
{
    public class RestaurantsRepository : IRestaurantsRepository
    {
        private readonly RestaurantsDbContext _dbContext;

        public RestaurantsRepository(RestaurantsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddAsync(Restaurant restaurant)
        {
            await _dbContext.AddAsync(restaurant);

            return restaurant.Id;
        }

        public async Task<IReadOnlyList<Restaurant>> GetAllAsync()
        {
            var restaurants = await _dbContext.Restaurants.ToListAsync();

            return restaurants;
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurant = await _dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefaultAsync(r => r.Id == id);

            return restaurant;
        }

        public Task DeleteAsync(Restaurant restaurant)
        {
            _dbContext.Restaurants.Remove(restaurant);

            return Task.CompletedTask;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var restaurnt = await _dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
            if (restaurnt != null)
                await DeleteAsync(restaurnt);
        }

        public async Task<int> CountByOwnerIdAsync(string userId)
        {
            var count = await _dbContext.Restaurants.CountAsync(r => r.OwnerId == userId);
            return count;
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
