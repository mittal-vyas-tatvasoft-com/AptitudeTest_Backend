﻿
using AptitudeTest.Core.Validations;
using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.ViewModels
{
    public class CreateTestVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TestDuration { get; set; }
        [DateMustBeValid]
        public DateTime Date { get; set; }
        [EndTimeGreaterThanStartTime]
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        [Range(0, 100)]
        public int NegativeMarkingPercentage { get; set; }
        public int BasicPoint { get; set; }
        [Range(1, 3)]
        public int Status { get; set; }
        public string? MessaageAtStartOfTheTest { get; set; }
        public string? MessaageAtEndOfTheTest { get; set; }
        public bool IsRandomQuestion { get; set; }
        public bool IsRandomAnswer { get; set; }
        public bool IsLogoutWhenTimeExpire { get; set; }
        public bool IsQuestionsMenu { get; set; }
        public int? CreatedBy { get; set; }
    }
}
