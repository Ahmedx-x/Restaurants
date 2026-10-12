using FluentValidation;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private string[] allowedSortByProperties = [
        nameof(Restaurant.Name),
        nameof(Restaurant.Description),
        nameof(Restaurant.Category)
    ];

    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.PageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0.")
            .LessThanOrEqualTo(30).WithMessage("Page size must not exceed 30.");
        RuleFor(x => x.SortBy)
            .Must(value => string.IsNullOrEmpty(value) || allowedSortByProperties.Contains(value))
            .WithMessage($"SortBy must be one of the following: {string.Join(", ", allowedSortByProperties)}.");
    }
}
