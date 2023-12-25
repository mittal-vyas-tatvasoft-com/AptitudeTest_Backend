using CsvHelper.Configuration.Attributes;

namespace AptitudeTest.Core.ViewModels
{
    public class ImportQuestionCsvVM
    {
        [Name("Question Number")]
        public int quetionnumber { get; set; }
        [Name("Version")]
        public char? version { get; set; }
        [Name("Topic")]
        public string? topic { get; set; }
        [Name("Marks")]
        public int difficulty { get; set; }
        [Name("Question")]
        public string questiontext { get; set; }
        [Name("Option A")]
        public string optiondata1 { get; set; }
        [Name("Option B")]
        public string optiondata2 { get; set; }
        [Name("Option C")]
        public string optiondata3 { get; set; }
        [Name("Option D")]
        public string optiondata4 { get; set; }
        [Name("Correct Option")]
        public char correctoption { get; set; }
        [Name("Status")]
        public bool status { get; set; }
    }
}
