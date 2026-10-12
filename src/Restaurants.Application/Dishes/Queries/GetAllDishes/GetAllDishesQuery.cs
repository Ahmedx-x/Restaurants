using MediatR;
using Restaurants.Application.Dtos.Dishes;

namespace Restaurants.Application.Dishes.Queries.GetAllDishes;

public class GetAllDishesQuery(int restaurantId) : IRequest<IReadOnlyList<DishDto>>
{
    public int RestaurantId { get; } = restaurantId;
}
