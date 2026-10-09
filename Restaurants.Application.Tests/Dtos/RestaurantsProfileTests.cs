using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Application.Mappers.Restaurants;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Tests.Dtos;

public class RestaurantsProfileTests
{
    private readonly IMapper _mapper;

    public RestaurantsProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
            cfg.AddProfile<RestaurantsProfile>()
        );
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Restaurant_To_RestaurantDto_Maps_AddressFields_When_Address_Is_Present()
    {
        // Arrange
        var restaurant = new Restaurant
        {
            Id = 1,
            Name = "R",
            Description = "D",
            Category = "C",
            HasDelivery = true,
            Address = new Address
            {
                City = "TestCity",
                Street = "TestStreet",
                PostalCode = "12345"
            }
        };

        // Act
        var dto = _mapper.Map<RestaurantDto>(restaurant);

        // Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(restaurant.Id);
        dto.Name.Should().Be(restaurant.Name);
        dto.Description.Should().Be(restaurant.Description);
        dto.Category.Should().Be(restaurant.Category);
        dto.HasDelivery.Should().Be(restaurant.HasDelivery);
        dto.City.Should().Be(restaurant.Address.City);
        dto.Street.Should().Be(restaurant.Address.Street);
        dto.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }

    [Fact]
    public void Restaurant_To_RestaurantDto_Maps_Null_Address_To_Null_Fields()
    {
        var restaurant = new Restaurant
        {
            Id = 2,
            Name = "R2",
            Description = "D2",
            Category = "C2",
            HasDelivery = false,
            Address = null
        };

        var dto = _mapper.Map<RestaurantDto>(restaurant);

        Assert.Equal(restaurant.Id, dto.Id);
        Assert.Equal(restaurant.Name, dto.Name);
        Assert.Equal(restaurant.Description, dto.Description);
        Assert.Equal(restaurant.Category, dto.Category);
        Assert.Equal(restaurant.HasDelivery, dto.HasDelivery);
        Assert.Null(dto.City);
        Assert.Null(dto.Street);
        Assert.Null(dto.PostalCode);
    }

    [Fact]
    public void CreateRestaurantCommand_Maps_To_Restaurant_With_Address()
    {
        var command = new CreateRestaurantCommand
        {
            Name = "New",
            Description = "Desc",
            Category = "Cat",
            HasDelivery = true,
            City = "CreateCity",
            Street = "CreateStreet",
            PostalCode = "C123"
        };

        var restaurant = _mapper.Map<Restaurant>(command);

        Assert.NotNull(restaurant);
        Assert.Equal(command.Name, restaurant.Name);
        Assert.Equal(command.Description, restaurant.Description);
        Assert.Equal(command.Category, restaurant.Category);
        Assert.Equal(command.HasDelivery, restaurant.HasDelivery);
        Assert.NotNull(restaurant.Address);
        Assert.Equal(command.City, restaurant.Address!.City);
        Assert.Equal(command.Street, restaurant.Address.Street);
        Assert.Equal(command.PostalCode, restaurant.Address.PostalCode);
    }

    [Fact]
    public void UpdateRestaurantCommand_Maps_To_Restaurant_With_Address()
    {
        var command = new UpdateRestaurantCommand
        {
            Id = 5,
            Name = "Upd",
            Description = "UpdDesc",
            Category = "UpdCat",
            HasDelivery = false,
            City = "UpdateCity",
            Street = "UpdateStreet",
            PostalCode = "U987"
        };

        var restaurant = _mapper.Map<Restaurant>(command);

        Assert.NotNull(restaurant);
        Assert.Equal(command.Id, restaurant.Id);
        Assert.Equal(command.Name, restaurant.Name);
        Assert.Equal(command.Description, restaurant.Description);
        Assert.Equal(command.Category, restaurant.Category);
        Assert.Equal(command.HasDelivery, restaurant.HasDelivery);
        Assert.NotNull(restaurant.Address);
        Assert.Equal(command.City, restaurant.Address!.City);
        Assert.Equal(command.Street, restaurant.Address.Street);
        Assert.Equal(command.PostalCode, restaurant.Address.PostalCode);
    }
}
