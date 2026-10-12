using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
    {
        public CreateDishCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Dish name is required.")
                .MaximumLength(100).WithMessage("Dish name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative value.");

            RuleFor(x => x.KiloCalories)
                .GreaterThanOrEqualTo(0)
                .WithMessage("KiloCalories must be a non-negative value");
        }
    }
}
