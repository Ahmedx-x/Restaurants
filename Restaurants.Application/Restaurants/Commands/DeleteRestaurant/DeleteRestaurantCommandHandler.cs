using MediatR;
using Microsoft.Extensions.Logging;
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
            var restaurant = await _restaurants.GetByIdAsync(request.id);

            if (restaurant == null)
            {
                _logger.LogError("this restaurant is not exist!");
                return Unit.Value;
            }

            await _restaurants.DeleteByIdAsync(request.id);
            await _restaurants.SaveChanges();
            return Unit.Value;
        }
    }
}
