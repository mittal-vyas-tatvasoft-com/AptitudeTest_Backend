using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels.User
{
    public class UserAcademicsVm
    {
        public int DegreeSpecializationId { get; set; }
        public string? SchoolOrCollegeOrUniversity { get; set; }
        public int StreamId { get; set; }
        public float Grade { get; set; }
        public int? Maths { get; set; }
        public int? Physics { get; set; }

        public int DurationFromYear { get; set; }
        public int DurationFromMonth { get; set; }
        public int DurationToYear { get; set; }
        public int DurationToMonth { get; set; }
    }
}
