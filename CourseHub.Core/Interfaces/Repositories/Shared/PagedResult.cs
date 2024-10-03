namespace CourseHub.Core.Interfaces.Repositories.Shared;

public class PagedResult<T>
{
    public int TotalCount { get; init; }
    public short PageIndex { get; init; }
    public byte PageSize { get; init; }
    public int PageCount { get => (int)Math.Ceiling((double)TotalCount / PageSize); }
    public List<T> Items { get; init; }

    public PagedResult(int totalCount, short pageIndex, byte pageSize, List<T> items)
    {
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;
        Items = items;
    }

    public static PagedResult<T> GetEmpty() => new(0, 0, 0, new List<T>());
}
