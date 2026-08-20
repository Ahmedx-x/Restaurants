using MediatR;
using Restaurants.Application.Dtos.Restaurants;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurant;

public record GetRestaurantQuery(int id) : IRequest<RestaurantDto>;
