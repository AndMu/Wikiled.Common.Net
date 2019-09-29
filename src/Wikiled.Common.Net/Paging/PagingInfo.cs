namespace Wikiled.Common.Net.Paging
{
    public class PagingInfo
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public int Start => PageSize * (PageNumber - 1);

        public int End => (PageSize * PageNumber - 1);
    }
}
