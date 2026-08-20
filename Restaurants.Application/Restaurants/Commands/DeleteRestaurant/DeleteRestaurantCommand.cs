using MediatR;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public record DeleteRestaurantCommand(int id) : IRequest<Unit>;
