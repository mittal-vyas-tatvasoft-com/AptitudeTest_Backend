namespace AptitudeTest.Core.ViewModels.Master
{
    public class LocationVM
    {
        public int? Id { get; set; } = 0;
        public string Location { get; set; }
        public bool? Status { get; set; } = true;
        public int CollegeId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
