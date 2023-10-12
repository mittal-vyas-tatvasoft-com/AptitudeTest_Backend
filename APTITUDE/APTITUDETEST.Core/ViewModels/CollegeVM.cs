using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class CollegeVM
    {
        public int Id { get; set; } = 0;
        [StringLength(500)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Abbreviation { get; set; }
        public bool? Status { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }
}


