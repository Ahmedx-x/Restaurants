namespace Restaurants.Domain.Exceptions;

public class NotFoundException(string resourceType, string resourceId)
    : Exception($"{resourceType} with ID {resourceId} not found.")
{
}
