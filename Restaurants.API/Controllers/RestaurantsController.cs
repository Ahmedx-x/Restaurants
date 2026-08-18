using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Application.Restaurants;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantsService _restaurantsService;

        public RestaurantsController(IRestaurantsService restaurantsService)
        {
            _restaurantsService = restaurantsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurants = await _restaurantsService.GetAll();
            return Ok(restaurants);
        }

        [HttpGet("{id}:int")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var restaurant = await _restaurantsService.GetById(id);
            if (restaurant == null)
                return NotFound();

            return Ok(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> AddRestaurant(CreateRestaurantDto restaurant)
        {
            int id = await _restaurantsService.Add(restaurant);

            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
    }
}
