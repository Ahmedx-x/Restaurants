using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Restaurants.Application.Users;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null)
        {
            throw new UnauthorizedAccessException("User context is not available.");
        }
        if (user.Identity == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.FindAll(c => c.Type == ClaimTypes.Role)!.Select(r => r.Value);
        var nationality = user.FindFirst(c => c.Type == "Nationality")?.Value;
        var dateOfBirth = user.FindFirst(c => c.Type == "DateOfBirth")?.Value;
        var doB = dateOfBirth != null ?
            DateOnly.ParseExact(dateOfBirth, "yyyy-MM-dd") : (DateOnly?)null;

        return new CurrentUser(userId, email, roles, nationality, doB);
    }
}
