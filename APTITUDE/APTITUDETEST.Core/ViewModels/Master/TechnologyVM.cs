using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels.Master
{
    public class TechnologyVM
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        public bool? Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
