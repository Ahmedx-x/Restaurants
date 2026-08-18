using FluentValidation;
using Restaurants.Application.Dtos.Restaurants;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantValidator : AbstractValidator<CreateRestaurantDto>
{
    private readonly List<string> validCategories = ["Italian", "Mexican", "Egyptian", "Indian"];
    public CreateRestaurantValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Category)
            .Must(validCategories.Contains)
            .WithMessage("Invalid category. please choose valid category");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress().WithMessage("Please provide valid email address");
    }
}
