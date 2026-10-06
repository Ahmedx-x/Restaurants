using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RestaurantsController(ISender _mediator) : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        // [Authorize(Policy = PolicyNames.AtLeastOwnedTwoRestaurants)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllRestaurantsQuery query)
        {
            var restaurants = await _mediator.Send(query);

            return Ok(restaurants);
        }

        [HttpGet("{id}:int")]
        [Authorize(Policy = PolicyNames.HasNationality)]       // Policy-based authorization
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var command = new GetRestaurantQuery(id);
            var restaurant = await _mediator.Send(command);

            return Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Owner)]   // Role-based authorization
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddRestaurant(CreateRestaurantCommand command)
        {
            int id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateRestaurant(UpdateRestaurantCommand command)
        {
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
        {
            var command = new DeleteRestaurantCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
