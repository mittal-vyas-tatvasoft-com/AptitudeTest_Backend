namespace AptitudeTest.Core.ViewModels.Master
{
    public class StreamVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DegreeId { get; set; }
        public string? DegreeName { get; set; }
        public bool? Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
