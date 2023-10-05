namespace TestAPI.Shared.Parameters
{
    public class RequestParameters
    {
        const int maxPageSize = 50;
        public int CurrentPage { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string OrderBy { get; set; } = "Name ASC";
        public string SearchTerm { get; set; }
        public string PropertyName { get; set; } = "Name";
        public string EntityName { get; set; } = "User";
    }
}
