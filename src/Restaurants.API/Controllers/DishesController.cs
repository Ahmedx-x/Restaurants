using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDishes;
using Restaurants.Application.Dishes.Queries.GetAllDishes;
using Restaurants.Application.Dishes.Queries.GetDishById;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers
{
    [Route("api/restaurants/{restaurantId}/dishes")]
    [ApiController]
    [Authorize]
    public class DishesController(ISender mediator) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateDish(int restaurantId, [FromBody] CreateDishCommand command)
        {
            command.RestaurantId = restaurantId;
            var result = await mediator.Send(command);
            return CreatedAtAction(nameof(GetDishById), new { restaurantId, dishId = result }, null);
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.AtLeast20)]
        public async Task<IActionResult> GetAllDishes([FromRoute] int restaurantId)
        {
            var command = new GetAllDishesQuery(restaurantId);
            var dishes = await mediator.Send(command);

            return Ok(dishes);
        }

        [HttpGet("{dishId:int}")]
        public async Task<IActionResult> GetDishById([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var command = new GetDishByIdQuery(restaurantId, dishId);
            var dish = await mediator.Send(command);

            return Ok(dish);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllDishesForRestaurant([FromRoute] int restaurantId)
        {
            var command = new DeleteAllDishesCommand(restaurantId);
            await mediator.Send(command);
            return NoContent();
        }
    }
}
