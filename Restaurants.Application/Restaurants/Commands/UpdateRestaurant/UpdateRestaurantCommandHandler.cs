using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler(
        IRestaurantsRepository restaurantsRepository,
        IMapper mapper,
        ILogger<UpdateRestaurantCommandHandler> logger,
        IRestaurantAuthorizationService authorizationService) : IRequestHandler<UpdateRestaurantCommand>
    {
        public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating restaurant {@Restaurant}", request);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            if (!authorizationService.Authorize(restaurant, ResourceOperation.Update))
                throw new ForbidException();

            mapper.Map(request, restaurant);
            await restaurantsRepository.SaveChanges();
        }
    }
}
