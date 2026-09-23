using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public class DeleteAllDishesCommand(int restaurantId) : IRequest
{
    public int RestaurantId { get; } = restaurantId;
}
