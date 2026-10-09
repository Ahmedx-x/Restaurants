using FluentAssertions;
using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidatorTests
{
    private readonly CreateRestaurantCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_PassesValidation()
    {
        // Arrange
        var cmd = new CreateRestaurantCommand
        {
            Name = "Pasta Palace",
            Description = "Nice place",
            Category = "Italian",
            ContactEmail = "test@rest.com",
            PostalCode = "12345",
            HasDelivery = true,
            City = "Sample",
            Street = "Main St",
            ContactNumber = "0123456789"
        };

        // Act
        var result = _validator.TestValidate(cmd);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_NameTooShort_FailsForName()
    {
        // Arrange
        var cmd = new CreateRestaurantCommand
        {
            Name = "AB",
            Description = "desc",
            Category = "Italian",
            ContactEmail = "a@b.com",
            PostalCode = "12345"
        };

        // Act
        var result = _validator.TestValidate(cmd);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Validate_InvalidCategory_FailsWithExpectedMessage()
    {
        // Arrange
        var cmd = new CreateRestaurantCommand
        {
            Name = "Valid Name",
            Description = "desc",
            Category = "UnknownCuisine",
            ContactEmail = "a@b.com",
            PostalCode = "12345"
        };

        // Act
        var result = _validator.TestValidate(cmd);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Category);
    }

    [Fact]
    public void Validate_InvalidEmail_FailsWithExpectedMessage()
    {
        // Arrange
        var cmd = new CreateRestaurantCommand
        {
            Name = "Valid Name",
            Description = "desc",
            Category = "Mexican",
            ContactEmail = "not-an-email",
            PostalCode = "12345"
        };

        // Act
        var result = _validator.TestValidate(cmd);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
        result.Errors.Should().ContainSingle(e =>
            e.PropertyName == nameof(CreateRestaurantCommand.ContactEmail) &&
            e.ErrorMessage == "Please provide valid email address"
        );
    }

    [Fact]
    public void Validate_InvalidPostalCode_FailsWithExpectedMessage()
    {
        // Arrange
        var cmd = new CreateRestaurantCommand
        {
            Name = "Valid Name",
            Description = "desc",
            Category = "Indian",
            ContactEmail = "a@b.com",
            PostalCode = "12"
        };

        // Act
        var result = _validator.TestValidate(cmd);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Theory]
    [InlineData("Italian")]
    [InlineData("Mexican")]
    [InlineData("Egyptian")]
    [InlineData("Indian")]
    public void Validate_ValidCategory_PassesValidation(string category)
    {
        // Arrange
        var cmd = new CreateRestaurantCommand
        {
            Name = "Valid Name",
            Description = "desc",
            Category = category,
            ContactEmail = "test@rest.com",
            PostalCode = "12345"
        };

        // Act
        var result = _validator.TestValidate(cmd);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
