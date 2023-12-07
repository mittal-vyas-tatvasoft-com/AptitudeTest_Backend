namespace AptitudeTest.Core.ViewModels
{
    public class UserDetailsVM
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public long? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PermanentAddress1 { get; set; }
        public string? PermanentAddress2 { get; set; }
        public int? Pincode { get; set; }
        public string? CityName { get; set; }
        public int? State { get; set; }
        public string? StateName { get; set; }
        public int? UserGroup { get; set; }
        public string? GroupName { get; set; }
        public int? UserCollege { get; set; }
        public string? CollegeName { get; set; }
        public int? AppliedThrough { get; set; }
        public int? TechnologyInterestedIn { get; set; }
        public string? TechnologyName { get; set; }
        public int? ACPCMeritRank { get; set; }
        public int? GUJCETScore { get; set; }
        public float? JEEScore { get; set; }
        public int? Gender { get; set; }

        public bool? Status { get; set; }
        public int? CreatedYear { get; set; }

        public List<UserFamilyVM>? FamilyDetails { get; set; }
        public List<UserAcademicsVM>? AcademicsDetails { get; set; }

    }
}
