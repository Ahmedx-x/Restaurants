using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.UpdateUser;

namespace Restaurants.API.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController(ISender mediator) : ControllerBase
    {
        [HttpPatch("user")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
