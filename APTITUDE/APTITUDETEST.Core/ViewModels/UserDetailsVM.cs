using AptitudeTest.Core.Entities.Users;

namespace AptitudeTest.Core.ViewModels
{
    public class UserDetailsVM
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        public string Email { get; set; }
        public long? PhoneNumber { get; set; }
        public int Level { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PermanentAddress { get; set; }
        public int? UserGroup { get; set; }
        public string? GroupName { get; set; }
        public int? AppliedThrough { get; set; }
        public int? TechnologyInterestedIn { get; set; }
        public string? TechnologyName { get; set; }
        public int? ACPCMeritRank { get; set; }
        public int? GUJCETScore { get; set; }
        public int? JEEScore { get; set; }
        public int? Gender { get; set; }
        public int? PreferedLocation { get; set; }
        public string? RelationshipWithExistingEmployee { get; set; }
        public bool? Status { get; set; }
        public int? RoleId { get; set; }
        public List<UserFamilyVM> FamilyDetails { get; set; }
        public List<UserAcademicsVM> AcademicsDetails { get; set; }

    }
}
