using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
    {
        private readonly List<string> validCategories = ["Italian", "Mexican", "Egyptian", "Indian"];
        public UpdateRestaurantCommandValidator()
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

}
