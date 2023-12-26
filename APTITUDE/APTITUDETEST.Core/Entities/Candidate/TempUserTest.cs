using APTITUDETEST.Core.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AptitudeTest.Core.Entities.Candidate
{
    public class TempUserTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }
        [Required]
        [ForeignKey("Tests")]
        public int TestId { get; set; }
        public bool Status { get; set; }
        public bool IsFinished { get; set; }
        public int TimeRemaining { get; set; }
        public bool IsAdminApproved { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public virtual User Users { get; set; }
        public virtual Test.Test Tests { get; set; }
    }
}
