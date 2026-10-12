using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes
{
    public class DeleteAllDishesCommandHandler(ILogger<DeleteAllDishesCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        IRestaurantAuthorizationService authorizationService) : IRequestHandler<DeleteAllDishesCommand>
    {
        public async Task Handle(DeleteAllDishesCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting all dishes for restaurant with ID {RestaurantId}", request.RestaurantId);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!authorizationService.Authorize(restaurant, ResourceOperation.Delete))
                throw new ForbidException();

            await dishesRepository.Delete(restaurant.Dishes);
        }
    }
}
