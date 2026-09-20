using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurant
{
    public class GetRestaurantQueryHandler : IRequestHandler<GetRestaurantQuery, RestaurantDto>
    {
        private readonly IRestaurantsRepository _restaurants;
        private readonly ILogger<GetRestaurantQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetRestaurantQueryHandler(
            IRestaurantsRepository restaurants,
            ILogger<GetRestaurantQueryHandler> logger,
            IMapper mapper)
        {
            _restaurants = restaurants;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<RestaurantDto?> Handle(GetRestaurantQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get restaurant with Id {RestaurantId}", request.id);

            var restaurant = await _restaurants.GetByIdAsync(request.id)
                ?? throw new NotFoundException(nameof(Restaurant), request.id.ToString());

            var restaurantDto = _mapper.Map<RestaurantDto?>(restaurant);
            return restaurantDto;
        }
    }
}
