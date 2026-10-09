using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Egyptian", "Indian"];
    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Category)
            .Must(validCategories.Contains)
            .WithMessage("Invalid category. please choose valid category");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress().WithMessage("Please provide valid email address");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{5}$")
            .WithMessage("Postal code must be a 5-digit number");
    }
}
