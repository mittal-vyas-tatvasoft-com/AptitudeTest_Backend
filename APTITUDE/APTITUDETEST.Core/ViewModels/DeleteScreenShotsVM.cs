using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class DeleteScreenShotsVM
    {
        [Required]
        public int TestId { get; set; }
        public int? UserId { get; set; }
        [Range(1, 4)]
        public int Level { get; set; }
        public string? FileName { get; set; }
        [Range(1, 2)]
        public int? Folder { get; set; }
    }
}
