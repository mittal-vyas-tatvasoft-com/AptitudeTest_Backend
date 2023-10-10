
using AptitudeTest.Core.Entities.Master;
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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        [Required]
        public string Email { get; set; }
        public long? PhoneNumber { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string Password { get; set; }
        public int Level { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? PermanentAddress { get; set; }
        [ForeignKey("MasterGroups")]
        public int? Group { get; set; }
        public int? AppliedThrough { get; set; }
        [ForeignKey("MasterTechnologies")]
        public int? TechnologyInterestedIn { get; set; }
        public int? ACPCMeritRank { get; set; }
        public int? GUJCETScore { get; set; }
        public int? JEEScore { get; set; }
        public int? Gender { get; set; }
        public int? PreferedLocation { get; set; }
        public string? RelationshipWithExistingEmployee { get; set; }
        public bool? Status { get; set; }
        public int? RoleId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual MasterGroup MasterGroups { get; set; }
        public virtual MasterTechnology MasterTechnologies { get; set; }

    }
}
