namespace AptitudeTest.Core.ViewModels
{
    public class CollegeQueryVM
    {
        public string? SearchQuery { get; set; }
        public int? Filter { get; set; }
        public int? CurrentPageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
