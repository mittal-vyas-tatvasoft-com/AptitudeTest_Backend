using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AptitudeTest.Core.Entities.Test
{
    public class Test
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public int TestDuration { get; set; }

        [ForeignKey("MasterGroup")]
        public int? GroupId { get; set; }
        public int NegativeMarkingPercentage { get; set; }
        public int BasicPoint { get; set; }
        public string? MessaageAtStartOfTheTest { get; set; }
        public string? MessaageAtEndOfTheTest { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        [Required]
        public int Status { get; set; }
        public bool? IsRandomQuestion { get; set; }
        public bool? IsRandomAnswer { get; set; }
        public bool? IsLogoutWhenTimeExpire { get; set; }
        public bool? IsQuestionsMenu { get; set; }
    }
}
