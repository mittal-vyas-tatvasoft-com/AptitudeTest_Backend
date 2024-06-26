﻿namespace AptitudeTest.Data.Common
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

        public enum SequenceStart
        {
            Start = 65
        }

        public enum DefaultQuestionId
        {
            QuestionId = -1
        }

        public enum QuestionStatus
        {
            UnVisited = 0,
            Answered = 1,
            Skipped = 2
        }

        public enum FieldsCount
        {
            ImportQuestionFieldsCount = 10
        }

        public enum ImageType
        {
            ScreenShot = 1,
            FaceCam = 2
        }

        public enum DirectoryLevel
        {
            Test = 1,
            User = 2,
            Folder = 3,
            Image = 4
        }

        public enum CollegeStatus
        {
            InActive = 1,
            Exists = 2,
            NotExists = 3,
        }
        public enum PreferredLocation
        {
            OnlyAhmedabad = 1,
            PreferredRajkot = 2,
            OnlyRajkot = 3
        }

    }
}
