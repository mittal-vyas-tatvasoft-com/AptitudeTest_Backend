using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AptitudeTest.Core.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? FatherName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public long? PhoneNumber { get; set; }

        public string Password { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int CollegeId { get; set; }
        [Required]
        public int Gender { get; set; }

        public bool Status { get; set; }
        //[Required]
        [Column(TypeName = "date")]
        public DateOnly? DateOfBirth { get; set; }
        //[Required]
        public string? PermanentAddress1 { get; set; }
        public string? PermanentAddress2 { get; set; }
        public long? Pincode { get; set; }
        public int? Level { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? AppliedThrough { get; set; }
        public int? TechnologyInterestedIn { get; set; }
        public string? RelationshipWithExistingEmployee { get; set; }
        public int? ACPCMeritRank { get; set; }
        public int? GUJCETScore { get; set; }
        public int? JEEScore { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        //[Required]
        public List<DapperUserAcademicsVM> UserAcademicsVM { get; set; }
        //[Required]
        public List<DapperUserFamilyVM> UserFamilyVM { get; set; }

    }
}
