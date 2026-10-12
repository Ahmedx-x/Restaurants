using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements.MinimumNumberOfRestaurants;

public class MinimumNumberOfCreatedRestaurantsRequirement(int minimumRestaurantsCreated) : IAuthorizationRequirement
{
    public int MinimumRestaurantsCreated { get; } = minimumRestaurantsCreated;
}
