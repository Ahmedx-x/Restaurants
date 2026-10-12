using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidCommand_ReturnsRestaurantId()
    {
        // Arrange
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Restaurant>()))
            .ReturnsAsync(1); // Simulate returning a restaurant ID

        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

        var mapperMock = new Mock<IMapper>();
        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();
        mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "test@rest.com", [], null, null);
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateRestaurantCommandHandler(restaurantsRepositoryMock.Object,
            loggerMock.Object,
            mapperMock.Object,
            userContextMock.Object
        );


        // Act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner-id");
        restaurantsRepositoryMock.Verify(r => r.AddAsync(restaurant), Times.Once);
    }
}
