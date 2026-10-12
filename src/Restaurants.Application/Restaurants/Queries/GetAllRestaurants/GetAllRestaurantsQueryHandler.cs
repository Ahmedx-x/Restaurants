using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler(
        IRestaurantsRepository restaurantsRepository,
        ILogger<GetAllRestaurantsQueryHandler> logger,
        IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
    {
        public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all restuarants");

            var (restaurants, totalCount) = await restaurantsRepository.GetAllMatchingAsync(
                request.SearchPhrase,
                request.PageNumber,
                request.PageSize,
                request.SortBy,
                request.SortType
            );

            var restaurantDtos = mapper.Map<IReadOnlyList<RestaurantDto?>>(restaurants);

            var pagedResult = new PagedResult<RestaurantDto>(
                restaurantDtos,
                totalCount,
                request.PageNumber,
                request.PageSize
            );

            return pagedResult;
        }
    }
}
