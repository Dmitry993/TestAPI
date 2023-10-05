namespace TestAPI.Shared
{
    public class PaginatedResult<T>
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IReadOnlyList<T> Items { get; set; }
    }
}
