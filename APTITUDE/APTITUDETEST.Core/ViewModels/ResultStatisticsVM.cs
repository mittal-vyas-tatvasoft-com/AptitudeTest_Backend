
namespace AptitudeTest.Core.ViewModels
{
    public class ResultStatisticsVM
    {
        public string StatisticsHeader { get; set; }
        public Double Points { get; set; }
        public string Correct { get; set; }
        public string Wrong { get; set; }
        public double UnAnswered { get; set; }
        public double UnDisplayed { get; set; }
    }
}
