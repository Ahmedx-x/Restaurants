using MediatR;
using Restaurants.Application.Dtos.Dishes;

namespace Restaurants.Application.Dishes.Queries.GetDishById;

public class GetDishByIdQuery(int restaurantId, int dishId) : IRequest<DishDto>
{
    public int RestaurantId { get; } = restaurantId;
    public int DishId { get; } = dishId;
}
