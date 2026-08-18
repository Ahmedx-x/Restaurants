namespace Restaurants.Application.Dtos.Restaurants;

public class CreateRestaurantDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Category { get; set; } = null!;
    public bool HasDelivery { get; set; }

    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }

    public string? ContactEmail { get; set; }
    public string? ContactNumber { get; set; }
}
