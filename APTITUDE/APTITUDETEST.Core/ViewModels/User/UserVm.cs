using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels.User
{
    public class UserVm
    {     
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        [Required]
        //public int? CollegeOrInstitute { get; set; }
        public string Email { get; set; }
        public long? PhoneNumber { get; set; }
        public string Password { get; set; }
        public int Level { get; set; }
        [Required]
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
        public List<UserAcademicsVm> UserAcademics { get; set; }
        [Required]
        public List<UserFamilyVm> UserFamily { get; set; } 
    }
}
