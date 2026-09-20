using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, Unit>
    {
        private readonly IRestaurantsRepository _restaurants;
        private readonly ILogger<DeleteRestaurantCommandHandler> _logger;

        public DeleteRestaurantCommandHandler(
            IRestaurantsRepository restaurants,
            ILogger<DeleteRestaurantCommandHandler> logger)
        {
            _restaurants = restaurants;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting restaurant with id {RestaurantId}", request.id);

            var restaurant = await _restaurants.GetByIdAsync(request.id)
                ?? throw new NotFoundException(nameof(Restaurant), request.id.ToString());

            await _restaurants.DeleteByIdAsync(request.id);
            await _restaurants.SaveChanges();
            return Unit.Value;
        }
    }
}
