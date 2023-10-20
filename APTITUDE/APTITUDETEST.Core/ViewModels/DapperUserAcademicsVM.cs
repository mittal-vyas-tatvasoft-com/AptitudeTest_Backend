using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels
{
    public class DapperUserAcademicsVM
    {
        public int userid { get; set; }
        public int degreeid { get; set; }
        public int streamid { get; set; }
        public int? maths { get; set; }
        public int? physics { get; set; }
        public string? university { get; set; }
        public float grade { get; set; }
        public int durationfromyear { get; set; }
        public int durationfrommonth { get; set; }
        public int durationtoyear { get; set; }
        public int durationtomonth { get; set; }
        public int createdby { get; set; }
    }
}
