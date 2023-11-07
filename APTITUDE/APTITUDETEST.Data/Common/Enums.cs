namespace AptitudeTest.Data.Common
{
    public class Enums
    {
        public enum QuestionTopic
        {
            Maths = 1,
            Reasoning = 2,
            Technical = 3
        }
        public enum OptionType
        {
            TextType = 1,
            ImageType = 2
        }
        public enum QuestionType
        {
            SingleAnswer = 1,
            MultiAnswer = 2
        }
        public enum TestStatus
        {
            Draft = 1,
            Active = 2,
            Completed = 3
        }

        public enum Pagination
        {
            DefaultIndex = 0,
            DefaultPageSize = 10
        }
    }
}
