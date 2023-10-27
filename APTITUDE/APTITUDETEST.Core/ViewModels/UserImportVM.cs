using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels
{
    public class UserImportVM
    {
        public string firstname { get; set; }       
        public string email { get; set; }
        public long contactnumber { get; set; }
    }
}
