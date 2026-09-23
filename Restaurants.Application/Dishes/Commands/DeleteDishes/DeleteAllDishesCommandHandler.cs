using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes
{
    public class DeleteAllDishesCommandHandler(ILogger<DeleteAllDishesCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository)
        : IRequestHandler<DeleteAllDishesCommand>
    {
        public async Task Handle(DeleteAllDishesCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting all dishes for restaurant with ID {RestaurantId}", request.RestaurantId);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant is null)
                throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            await dishesRepository.Delete(restaurant.Dishes);
        }
    }
}
