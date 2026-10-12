using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories
{
    public class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
    {
        public async Task<int> AddAsync(Restaurant restaurant)
        {
            await dbContext.AddAsync(restaurant);
            await SaveChanges();

            return restaurant.Id;
        }

        public async Task<IReadOnlyList<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants.AsNoTracking().ToListAsync();

            return restaurants;
        }

        public async Task<(IReadOnlyList<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase,
            int pageNumber, int pageSize, string? sortBy, SortType sortType)
        {

            IQueryable<Restaurant> baseQuery = dbContext.Restaurants;

            if (!string.IsNullOrEmpty(searchPhrase))
                baseQuery = ApplyFiltering(baseQuery, searchPhrase);

            var totalCount = await baseQuery.CountAsync();

            if (!string.IsNullOrEmpty(sortBy))
                baseQuery = ApplySorting(baseQuery, sortBy, sortType);

            var matchingRestaurants = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (matchingRestaurants, totalCount);
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurant = await dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefaultAsync(r => r.Id == id);

            return restaurant;
        }

        public async Task DeleteAsync(Restaurant restaurant)
        {
            dbContext.Restaurants.Remove(restaurant);
            await SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var restaurnt = await dbContext.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
            if (restaurnt != null)
                await DeleteAsync(restaurnt);
        }

        public async Task<int> CountByOwnerIdAsync(string userId)
        {
            var count = await dbContext.Restaurants.CountAsync(r => r.OwnerId == userId);
            return count;
        }

        public async Task SaveChanges()
        {
            await dbContext.SaveChangesAsync();
        }

        private IQueryable<Restaurant> ApplyFiltering(IQueryable<Restaurant> query, string searchPhrase)
        {
            var searchPhraseLower = searchPhrase.ToLower();
            var filteredQuery = query.Where(
                r => r.Name.ToLower().Contains(searchPhraseLower) ||
                r.Description.ToLower().Contains(searchPhraseLower)
            );

            return filteredQuery;
        }

        private IQueryable<Restaurant> ApplySorting(IQueryable<Restaurant> query, string sortBy, SortType sortType)
        {
            var columnsSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category }
            };

            var selectedColumn = columnsSelectors[sortBy];

            var sortedQuery = sortType == SortType.Ascending
                ? query.OrderBy(selectedColumn)
                : query.OrderByDescending(selectedColumn);

            return sortedQuery;
        }
    }
}
