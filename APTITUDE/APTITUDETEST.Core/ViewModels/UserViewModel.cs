using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Entities.Users;

namespace AptitudeTest.Core.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? FatherName { get; set; }
        public long? PhoneNumber { get; set; }
        public int? Level { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PermanentAddress { get; set; }
        public MasterGroup? Group { get; set; }
        public int? AppliedThrough { get; set; }
        public MasterTechnology? TechnologyInterestedIn { get; set; }
        public int? ACPCMeritRank { get; set; }
        public int? GUJCETScore { get; set; }
        public int? JEEScore { get; set; }
        public int? Gender { get; set; }
        public int? PreferedLocation { get; set; }
        public string? RelationshipWithExistingEmployee { get; set; }
        public bool? Status { get; set; }
        public int? RoleId { get; set; }
        public string? DegreeName { get; set; }
        public string? Stream { get; set; }
        public float? Maths { get; set; }
        public float? Physics { get; set; }
        public float? Grade { get; set; }
        public string? University { get; set; }
        public int? DurationFromYear { get; set; }
        public int? DurationFromMonth { get; set; }
        public int? DurationToYear { get; set; }
        public int? DurationToMonth { get; set; }
        public int? FamilyPerson { get; set; }
        public string? Qualification { get; set; }
        public string? Occupation { get; set; }
        public UserFamily? UserFamilyData { get; set; }
        public UserAcademics? UserAcademics { get; set; }
    }
}
