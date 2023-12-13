using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels
{
    public class UserTestResultDataVM
    {
        public string Name { get; set; }
        public int QuestionId { get; set; }
        public int[] UserAnswers { get; set; }
        public bool IsAttended { get; set; }
        public int Difficulty { get; set; }
        public string? QuestionText { get; set; }
        public int QuestionType { get; set; }
        public int OptionType { get; set; }
        public int OptionId { get; set; }
        public string? OptionData { get; set; }
        public bool IsAnswer { get; set; }

    }
}
