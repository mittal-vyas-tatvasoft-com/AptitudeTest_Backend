using AptitudeTest.Core.Entities.Master;
using APTITUDETEST.Core.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AptitudeTest.Core.Entities.Users
{
    public partial class UserAcademics
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Users")]
        public int UserId { get; set; }
        [ForeignKey("MasterDegrees")]
        public int DegreeId { get; set; }
        [ForeignKey("MasterStreams")]
        public int StreamId { get; set; }
        public float? Maths { get; set; }
        public float? Physics { get; set; }
        public float Grade { get; set; }
        [Required]
        public string University { get; set; }
        public int DurationFromYear { get; set; }
        public int DurationFromMonth { get; set; }
        public int DurationToYear { get; set; }
        public int DurationToMonth { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual MasterDegree? MasterDegrees { get; set; }
        public virtual User? Users { get; set; }
        public virtual MasterStream? MasterStreams { get; set; }

    }
}
