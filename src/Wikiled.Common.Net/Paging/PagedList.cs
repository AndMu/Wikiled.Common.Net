using System;

namespace Wikiled.Common.Net.Paging
{
    public class PagedList<T>
    {
        public PagedList(T[] source, long count, PagingInfo info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            TotalItems = count;
            Data = source;
        }

        public PagingInfo Info { get; }

        public long TotalItems { get; }

        public T[] Data { get; }

        public int TotalPages => (int) Math.Ceiling(TotalItems / (double)Info.PageSize);

        public bool HasPreviousPage => Info.PageNumber > 1;

        public bool HasNextPage => Info.PageNumber < TotalPages;

        public int NextPageNumber => HasNextPage ? Info.PageNumber + 1 : TotalPages;

        public int PreviousPageNumber => HasPreviousPage ? Info.PageNumber - 1 : 1;
    }
}
