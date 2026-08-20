using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, Unit>
    {
        private readonly IRestaurantsRepository _restaurants;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateRestaurantCommandHandler> _logger;

        public UpdateRestaurantCommandHandler(
            IRestaurantsRepository restaurants,
            IMapper mapper,
            ILogger<UpdateRestaurantCommandHandler> logger)
        {
            _restaurants = restaurants;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update restaurant");

            var restaurant = await _restaurants.GetByIdAsync(request.Id);
            if (restaurant == null)
            {
                _logger.LogError("this restaurant not exist!");
                return Unit.Value;
            }

            _mapper.Map(request, restaurant);
            await _restaurants.SaveChanges();

            return Unit.Value;
        }
    }
}
