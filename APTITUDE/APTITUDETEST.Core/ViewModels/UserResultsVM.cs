using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Core.ViewModels
{
    public class UserResultsVM
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public int AllQuestionCount { get; set; }
        public int AllCorrectQuestionCount { get; set; }
        public int Marks1QuestionCount { get; set; }
        public int Marks1CorrectQuestionCount { get; set; }
        public int Marks2QuestionCount { get; set; }
        public int Marks2CorrectQuestionCount { get; set; }
        public int Marks3QuestionCount { get; set; }
        public int Marks3CorrectQuestionCount { get; set; }
        public int Marks4QuestionCount { get; set; }
        public int Marks4CorrectQuestionCount { get; set; }
        public int Marks5QuestionCount { get; set; }
        public int Marks5CorrectQuestionCount { get; set; }

        public PaginationVM<UserResultQuestionVM> PaginatedData { get; set; }=new PaginationVM<UserResultQuestionVM>();
    }
}
