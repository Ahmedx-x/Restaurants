using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
    {
        private readonly IRestaurantsRepository _restaurants;
        private readonly ILogger<CreateRestaurantCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateRestaurantCommandHandler(
            IRestaurantsRepository restaurants,
            ILogger<CreateRestaurantCommandHandler> logger,
            IMapper mapper)
        {
            _restaurants = restaurants;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Createing a new restaurant");

            return await _restaurants.AddAsync(_mapper.Map<Restaurant>(request));
        }
    }
}
