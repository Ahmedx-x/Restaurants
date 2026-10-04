using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.RemoveUserRole
{
    public class RemoveUserRoleCommandHandler(ILogger<RemoveUserRoleCommandHandler> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager) : IRequestHandler<RemoveUserRoleCommand>
    {
        public async Task Handle(RemoveUserRoleCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Removing role {RoleName} from user {UserEmail}", request.RoleName, request.UserEmail);

            var user = await userManager.FindByEmailAsync(request.UserEmail)
                ?? throw new NotFoundException(nameof(User), request.UserEmail);

            bool roleExists = await roleManager.RoleExistsAsync(request.RoleName);
            if (!roleExists)
            {
                throw new NotFoundException(nameof(IdentityRole), request.RoleName);
            }

            await userManager.RemoveFromRoleAsync(user, request.RoleName);
        }
    }
}
