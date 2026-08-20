using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurant;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly ISender _mediator;

        public RestaurantsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var command = new GetAllRestaurantsQuery();
            var restaurants = await _mediator.Send(command);

            return Ok(restaurants);
        }

        [HttpGet("{id}:int")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var command = new GetRestaurantQuery(id);
            var restaurant = await _mediator.Send(command);
            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> AddRestaurant(CreateRestaurantCommand command)
        {
            int id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateRestaurant(UpdateRestaurantCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            var command = new DeleteRestaurantCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
