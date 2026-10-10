using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumNumberOfRestaurants
{
    internal class MinimumNumberOfCreatedRestaurantsRequirementHandler(
        //ILogger<MinimumNumberOfCreatedRestaurantsRequirementHandler> logger,
        IUserContext userContext,
        IRestaurantsRepository restaurantsRepository) : AuthorizationHandler<MinimumNumberOfCreatedRestaurantsRequirement>
    {
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MinimumNumberOfCreatedRestaurantsRequirement requirement)
        {
            var currentUser = userContext.GetCurrentUser();
            //logger.LogInformation("Checking if user {UserId} has created at least {NumberOfRestaurantsCreated} restaurants",
            //    currentUser.Id, requirement.MinimumRestaurantsCreated);

            int numberOfRestaurants = await restaurantsRepository.CountByOwnerIdAsync(currentUser.Id);

            if (numberOfRestaurants >= requirement.MinimumRestaurantsCreated)
            {
                //logger.LogInformation("User {UserId} has created {CreatedRestaurantsCount} restaurants",
                //    currentUser.Id, numberOfRestaurants);
                context.Succeed(requirement);
            }
            else
            {
                //logger.LogWarning("User {UserId} has not created enough restaurants. Required: {NumberOfRestaurantsCreated}, Created: {CreatedRestaurantsCount}",
                //    currentUser.Id, requirement.MinimumRestaurantsCreated, numberOfRestaurants);
                context.Fail();
            }
        }
    }
}
