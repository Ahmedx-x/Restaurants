using MediatR;

namespace Restaurants.Application.Users.Commands.RemoveUserRole;

public class RemoveUserRoleCommand : IRequest
{
    public string UserEmail { get; set; } = null!;
    public string RoleName { get; set; } = null!;
}
