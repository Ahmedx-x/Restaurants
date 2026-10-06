namespace Restaurants.Application.Common;

public class PagedResult<T>
{
    public PagedResult(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        ItemsFrom = (pageNumber - 1) * pageSize + 1;
        ItemsTo = Math.Min(pageNumber * pageSize, totalCount);
    }

    public IReadOnlyList<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
}
