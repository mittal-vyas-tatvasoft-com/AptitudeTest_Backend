using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AptitudeTest.Core.Entities.Admin
{
    public partial class Admin
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
        public bool? Status { get; set; }
        public bool? IsSuperAdmin { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
