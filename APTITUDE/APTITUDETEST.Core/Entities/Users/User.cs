
using AptitudeTest.Core.Entities.Master;
using AptitudeTest.Core.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APTITUDETEST.Core.Entities.Users
{
    public partial class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FatherName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public long PhoneNumber { get; set; }
        public string? Password { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PermanentAddress1 { get; set; }
        public string? PermanentAddress2 { get; set; }
        public int? Pincode { get; set; }
        [ForeignKey("States")]
        public int? StateId { get; set; }
        public string? CityName { get; set; }
        [ForeignKey("MasterGroups")]
        public int? GroupId { get; set; }
        [ForeignKey("MasterColleges")]
        public int? CollegeId { get; set; }
        public int? AppliedThrough { get; set; }
        [ForeignKey("MasterTechnologies")]
        public int? TechnologyInterestedIn { get; set; }
        public int? ACPCMeritRank { get; set; }
        public float? GUJCETScore { get; set; }
        public float? JEEScore { get; set; }
        public int? Gender { get; set; }
        public int? PreferedLocation { get; set; }
        public string? RelationshipWithExistingEmployee { get; set; }
        public bool? Status { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string? SessionId { get; set; }
        public virtual MasterGroup MasterGroups { get; set; }
        public virtual MasterTechnology MasterTechnologies { get; set; }
        public virtual MasterCollege MasterColleges { get; set; }
        public virtual State States { get; set; }
        public bool IsImported { get; set; } = false;

    }
}
