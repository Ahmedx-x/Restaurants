using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
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
            _logger.LogInformation("Updating restaurant {@Restaurant}", request);

            var restaurant = await _restaurants.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            _mapper.Map(request, restaurant);
            await _restaurants.SaveChanges();

            return Unit.Value;
        }
    }
}
