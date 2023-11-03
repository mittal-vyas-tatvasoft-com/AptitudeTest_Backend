namespace AptitudeTest.Core.ViewModels
{
    public class AdminDataVM
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public long? PhoneNumber { get; set; }
        public bool? Status { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public int? CreatedYear { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
        public int? NextPage { get; set; }
    }
}
