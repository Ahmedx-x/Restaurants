using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly IRestaurantsRepository _restaurants;
        private readonly ILogger<RestaurantsService> _logger;
        private readonly IMapper _mapper;

        public RestaurantsService(
            IRestaurantsRepository restaurants,
            ILogger<RestaurantsService> logger,
            IMapper mapper)
        {
            _restaurants = restaurants;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> Add(CreateRestaurantDto restaurant)
        {
            _logger.LogInformation("Createing a new restaurant");
            return await _restaurants.AddAsync(_mapper.Map<Restaurant>(restaurant));
        }

        public async Task<IReadOnlyList<RestaurantDto>> GetAll()
        {
            _logger.LogInformation("Getting all restuarants");
            var restaurants = await _restaurants.GetAllAsync();

            var result = _mapper.Map<IReadOnlyList<RestaurantDto?>>(restaurants);
            return result!;
        }

        public async Task<RestaurantDto?> GetById(int id)
        {
            _logger.LogInformation($"Get restaurant with Id {id}");
            var restaurant = await _restaurants.GetByIdAsync(id);

            var restaurantDto = _mapper.Map<RestaurantDto?>(restaurant);
            return restaurantDto;
        }
    }
}
