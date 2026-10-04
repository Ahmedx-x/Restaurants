using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.RemoveUserRole;
using Restaurants.Application.Users.Commands.UpdateUser;
using Restaurants.Domain.Constants;

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

        [HttpPost("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AssignUserRole(AssignUserRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("userRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RemoveUserRole(RemoveUserRoleCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
