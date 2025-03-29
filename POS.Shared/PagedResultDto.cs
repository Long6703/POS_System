namespace POS.Shared
{
    public partial class PagedResultDto<T> where T : class
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
        public IEnumerable<T> Items { get; set; }

        public PagedResultDto(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Items = items;
        }
    }
}
