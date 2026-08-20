using MediatR;
using Restaurants.Application.Dtos.Restaurants;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public record GetAllRestaurantsQuery : IRequest<IReadOnlyList<RestaurantDto>>;
