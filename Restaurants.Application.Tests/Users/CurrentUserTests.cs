using FluentAssertions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests.Users;

public class CurrentUserTests
{
    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string role)
    {
        // Arrange
        var user = new CurrentUser("1", "user@example.com", [UserRoles.Admin, UserRoles.User], null, null);
        // Act
        var result = user.IsInRole(role);
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInRole_WithMatchingRole_ShouldReturnFalse()
    {
        // Arrange
        var user = new CurrentUser("1", "user@example.com", [UserRoles.Admin, UserRoles.User], null, null);
        // Act
        var result = user.IsInRole(UserRoles.Owner);
        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsInRole_WithNonMatchingRole_ShouldReturnFalse()
    {
        // Arrange
        var user = new CurrentUser("1", "user@example.com", [UserRoles.Admin, UserRoles.User], null, null);
        // Act
        var result = user.IsInRole(UserRoles.Admin.ToLower());
        // Assert
        result.Should().BeFalse();
    }
}