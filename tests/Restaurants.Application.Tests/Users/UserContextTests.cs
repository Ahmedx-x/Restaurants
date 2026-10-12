using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using System.Security.Claims;

namespace Restaurants.Application.Tests.Users;

public class UserContextTests
{
    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        // Arrange
        var dateOfBirth = new DateOnly(2003, 11, 15);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userContext = new UserContext(httpContextAccessorMock.Object);

        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@rest.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "USA"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = user });

        // Act
        var currentUser = userContext.GetCurrentUser();

        // Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@rest.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("USA");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);

    }

    [Fact]
    public void GetCurrentUser_WithUnauthenticatedUser_ShouldThrowUnauthorizedAccessException()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userContext = new UserContext(httpContextAccessorMock.Object);
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = user });

        // Act
        Action act = () => userContext.GetCurrentUser();

        // Assert
        act.Should().Throw<UnauthorizedAccessException>().WithMessage("User is not authenticated.");
    }

    [Fact]
    public void GetCurrentUser_WithNoHttpContext_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userContext = new UserContext(httpContextAccessorMock.Object);
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        // Act
        Action act = () => userContext.GetCurrentUser();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("User context is not available.");
    }
}
