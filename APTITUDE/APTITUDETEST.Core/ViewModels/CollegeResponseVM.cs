using AptitudeTest.Core.Entities.Master;

namespace AptitudeTest.Core.ViewModels
{
    public class CollegeResponseVM
    {
        public List<MasterCollege> CollegeList { get; set; }
        public int CurrentPageIndex { get; set; }
        public bool IsPreviousPage { get; set; }
        public bool IsNextPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }

    }
}
