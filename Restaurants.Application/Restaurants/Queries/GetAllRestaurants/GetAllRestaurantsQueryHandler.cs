using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler :
        IRequestHandler<GetAllRestaurantsQuery, IReadOnlyList<RestaurantDto>>
    {
        private readonly IRestaurantsRepository _restaurants;
        private readonly ILogger<GetAllRestaurantsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllRestaurantsQueryHandler(
            IRestaurantsRepository restaurants,
            ILogger<GetAllRestaurantsQueryHandler> logger,
            IMapper mapper)
        {
            _restaurants = restaurants;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all restuarants");

            var restaurants = await _restaurants.GetAllAsync();
            var result = _mapper.Map<IReadOnlyList<RestaurantDto?>>(restaurants);
            return result!;
        }
    }
}
