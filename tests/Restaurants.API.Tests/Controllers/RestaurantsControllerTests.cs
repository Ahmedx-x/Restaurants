using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;
using System.Net;
using System.Net.Http.Json;

namespace Restaurants.API.Tests.Controllers
{
    public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
        private readonly Mock<IRestaurantSeeder> _restaurantsSeederMock = new();

        public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
        {
            _restaurantsSeederMock
                .Setup(seeder => seeder.Seed())
                .Returns(Task.CompletedTask);

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                    services.RemoveAll<IRestaurantSeeder>();
                    services.AddSingleton(_restaurantsSeederMock.Object);

                    services.Replace(
                        ServiceDescriptor.Scoped(
                            typeof(IRestaurantsRepository),
                            _ => _restaurantsRepositoryMock.Object));
                });
            });
        }

        [Fact]
        public async Task GetAll_ForValidRequest_Returns200Ok()
        {
            // arrange
            var client = _factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/restaurants?PageNumber=1&PageSize=5");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange
            var client = _factory.CreateClient();

            // act
            var result = await client.GetAsync("/api/restaurants?PageSize=-1");

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task GetById_ForExistingId_Returns200Ok()
        {
            // arrange
            var id = 1;

            var restaurant = new Restaurant
            {
                Id = id,
                Name = "Test",
                Category = "Egyptian"
            };

            _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(restaurant);

            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync($"/api/restaurants/{id}");
            var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            restaurantDto.Should().NotBeNull();
            restaurantDto.Id.Should().Be(id);
            restaurantDto.Name.Should().Be(restaurant.Name);
            restaurantDto.Category.Should().Be(restaurant.Category);
        }

        [Fact]
        public async Task GetById_ForNonExistingId_Returns404NotFound()
        {
            // arrange
            var id = 4;

            var client = _factory.CreateClient();

            _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

            // act
            var response = await client.GetAsync($"/api/restaurants/{id}");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
