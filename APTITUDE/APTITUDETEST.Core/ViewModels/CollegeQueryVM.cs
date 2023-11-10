namespace AptitudeTest.Core.ViewModels
{
    public class CollegeQueryVM
    {
        public int? CurrentPageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? sortField { get; set; }
        public string? sortOrder { get; set; }
    }
}
