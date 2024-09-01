namespace Wikiled.Common.Net.Paging;

public record PagingInfo
{
    public int PageNumber { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public int Start => PageSize * (PageNumber - 1);

    public int End => (PageSize * PageNumber) - 1;
}