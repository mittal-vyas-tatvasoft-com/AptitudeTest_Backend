using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class DegreeVM
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        public List<string> Streams { get; set; }
        [Range(0, 5)]
        public int Level { get; set; }
        public bool? Status { get; set; }
        public bool? IsEditable { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
