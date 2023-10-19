using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels
{
    public class UserAcademicsVM
    {
        public int AcademicId { get; set; }
        public int DegreeId { get; set; }
        public int StreamId { get; set; }
        public float Maths { get; set; }
        public float Physics { get; set; }
        public float Grade { get; set; }
        public string University { get; set; }
        public int DurationFromYear { get; set; }
        public int DurationFromMonth { get; set; }
        public int DurationToYear { get; set; }
        public int DurationToMonth { get; set; }
        public string DegreeName { get; set; }
        public int DegreeLevel { get; set; }
        public string StreamName { get; set; }
    }
}
