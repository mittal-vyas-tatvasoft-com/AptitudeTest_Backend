namespace AptitudeTest.Core.ViewModels
{
    public class PaginationVM<T>
    {
        public List<T>? EntityList { get; set; } = new List<T>();
        public int CurrentPageIndex { get; set; }
        public bool IsPreviousPage { get; set; }
        public bool IsNextPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int TotalItemsCount { get; set; }
    }
}
