using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class TestStatusVM
    {
        public int Id { get; set; }
        [Range(1, 3)]
        public int Status { get; set; }
    }
}
