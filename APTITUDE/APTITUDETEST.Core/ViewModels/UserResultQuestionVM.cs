using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels
{
    public class UserResultQuestionVM
    {
        public int Id { get; set; }
        public int Difficulty { get; set; }
        public string QuestionText { get; set; }=String.Empty;
        public int OptionType { get; set; }
        public List<int> UserAnswers { get; set; }=new List<int>();
        public List<UserResultOptionVM> Options { get; set; } = new List<UserResultOptionVM>();
    }
}
