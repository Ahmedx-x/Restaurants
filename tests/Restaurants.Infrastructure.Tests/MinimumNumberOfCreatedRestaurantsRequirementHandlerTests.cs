using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements.MinimumNumberOfRestaurants;

namespace Restaurants.Infrastructure.Tests
{
    public class MinimumNumberOfCreatedRestaurantsRequirementHandlerTests
    {
        [Fact]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
        {
            // Arrange
            //var loggerMock = new Mock<ILogger<MinimumNumberOfCreatedRestaurantsRequirementHandler>>();

            var currentUser = new CurrentUser("1", "test@rest.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(repo => repo.CountByOwnerIdAsync(currentUser.Id)).ReturnsAsync(2);

            var requirement = new MinimumNumberOfCreatedRestaurantsRequirement(2);

            var handler = new MinimumNumberOfCreatedRestaurantsRequirementHandler(
                //loggerMock.Object,
                userContextMock.Object,
                restaurantRepositoryMock.Object
            );

            var context = new AuthorizationHandlerContext([requirement], null, null);

            // Act
            await handler.HandleAsync(context);

            // Assert
            context.HasSucceeded.Should().BeTrue();
        }

        [Fact]
        public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
        {
            // Arrange
            //var loggerMock = new Mock<ILogger<MinimumNumberOfCreatedRestaurantsRequirementHandler>>();

            var currentUser = new CurrentUser("1", "test@rest.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(repo => repo.CountByOwnerIdAsync(currentUser.Id)).ReturnsAsync(1);

            var requirement = new MinimumNumberOfCreatedRestaurantsRequirement(2);

            var handler = new MinimumNumberOfCreatedRestaurantsRequirementHandler(
                //loggerMock.Object,
                userContextMock.Object,
                restaurantRepositoryMock.Object
            );

            var context = new AuthorizationHandlerContext([requirement], null, null);

            // Act
            await handler.HandleAsync(context);

            // Assert
            context.HasFailed.Should().BeTrue();
        }
    }
}