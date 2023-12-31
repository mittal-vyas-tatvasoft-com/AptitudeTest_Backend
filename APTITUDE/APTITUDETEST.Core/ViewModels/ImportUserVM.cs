﻿using AptitudeTest.Core.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class ImportUserVM
    {
        [Required(ErrorMessage = "Please select a file.")]
        [FileExtensionMustBeValid(".txt", ".csv")] // Add the allowed extensions you want to allow
        public IFormFile file { get; set; }
        public int? GroupId { get; set; } = 0;
        public int? CollegeId { get; set; } = 0;

    }
}
