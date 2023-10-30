namespace AptitudeTest.Core.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public long? PhoneNumber { get; set; }
        public string? GroupName { get; set; }
        public string? CollegeName { get; set; }
        public bool? Status { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalPages { get; set; }
        public int? NextPage { get; set; }
    }
}
