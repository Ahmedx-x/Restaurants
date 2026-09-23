using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos.Dishes;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetAllDishes
{
    public class GetAllDishesQueryHandler(ILogger<GetAllDishesQueryHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper)
        : IRequestHandler<GetAllDishesQuery, IReadOnlyList<DishDto>>
    {
        public async Task<IReadOnlyList<DishDto>> Handle(GetAllDishesQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Retrieving dishes for restaurant with ID {RestaurantId}", request.RestaurantId);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            var dishes = mapper.Map<IReadOnlyList<DishDto>>(restaurant.Dishes);
            return dishes;
        }
    }
}
