using AptitudeTest.Core.Validations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels
{
    public class UserResultOptionVM
    {
        public int OptionId { get; set; }
        public string? OptionValue { get; set; }
        public bool IsAnswer { get; set; }
        public bool IsUserAnswer { get; set; }
    }
}
