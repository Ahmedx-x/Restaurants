using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumAgeRequirement
{
    internal class MinimumAgeRequirementHandler(
        ILogger<MinimumAgeRequirementHandler> logger,
        IUserContext userContext)
        : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MinimumAgeRequirement requirement)
        {
            var currentUser = userContext.GetCurrentUser();

            logger.LogInformation("Checking minimum age requirement for user {UserEmail} with date of birth {DoB}",
                currentUser?.Email, currentUser?.DateOfBirth);

            if (currentUser?.DateOfBirth == null)
            {
                logger.LogWarning("User {UserEmail} does not have a date of birth. Failing requirement.",
                    currentUser?.Email);
                context.Fail();
                return Task.CompletedTask;
            }

            if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
            {
                logger.LogInformation("User {UserEmail} meets the minimum age requirement of {MinimumAge}.",
                    currentUser.Email, requirement.MinimumAge);
                context.Succeed(requirement);
            }
            else
            {
                logger.LogWarning("User {UserEmail} does not meet the minimum age requirement of {MinimumAge}.",
                    currentUser.Email, requirement.MinimumAge);
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
