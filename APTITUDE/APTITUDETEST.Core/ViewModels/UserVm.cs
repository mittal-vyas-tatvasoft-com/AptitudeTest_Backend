using AptitudeTest.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        [Required]
        public string Email { get; set; }
        public long? PhoneNumber { get; set; }
        public string Password { get; set; }
        public int Level { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateOnly? DateOfBirth { get; set; }
        [Required]
        public string? PermanentAddress { get; set; }
        [Required]
        public int Group { get; set; }
        public int? AppliedThrough { get; set; }
        public int? TechnologyInterestedIn { get; set; }
        public int? PreferedLocation { get; set; }
        public string? RelationshipWithExistingEmployee { get; set; }
        public int? ACPCMeritRank { get; set; }
        public int? GUJCETScore { get; set; }
        public int? JEEScore { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        [Required]
        public List<DapperUserAcademicsVM> UserAcademicsVM { get; set; }
        [Required]
        public List<DapperUserFamilyVM> UserFamilyVM { get; set; }
        
    }
}
