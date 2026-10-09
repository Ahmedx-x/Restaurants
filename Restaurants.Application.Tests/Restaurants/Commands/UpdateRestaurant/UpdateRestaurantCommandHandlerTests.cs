using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(
            _restaurantsRepositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _restaurantAuthorizationServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurants()
    {
        // arrange
        // simulate the Update command
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Name = "New Test",
            Description = "New Description",
            HasDelivery = true,
            Category = "Egyptian"
        };

        // simulate the Restaurant entity returned from db
        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Name = "Test",
            Description = "Test",
            Category = "Indian"
        };

        _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(auth => auth.Authorize(restaurant, ResourceOperation.Update))
            .Returns(true);

        // act
        await _handler.Handle(command, CancellationToken.None);

        // assert
        _restaurantsRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once);
        _mapperMock.Verify(repo => repo.Map(command, restaurant), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Category = "Egyptian"
        };

        _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
            .ReturnsAsync((Restaurant?)null);

        // act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with ID {restaurantId} not found.");
    }

    [Fact]
    public async Task Handle_WithUnauthorizedUser_ShouldThrowForbidException()
    {
        // arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Description = "New Description",
        };

        var restaurant = new Restaurant()
        {
            Id = restaurantId,
            Description = "Old",
        };

        _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(auth => auth.Authorize(restaurant, ResourceOperation.Update))
            .Returns(false);

        // act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // assert
        await act.Should().ThrowAsync<ForbidException>();
    }
}