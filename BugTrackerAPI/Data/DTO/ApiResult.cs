namespace BugTrackerAPI.Data.DTO;

public class ApiResult<T>
{
    public List<T> Data { get; private set; }
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }

    public ApiResult(List<T> data, int pageIndex, int pageSize, int totalCount, int totalPages)
    {
        Data = data;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }
}
