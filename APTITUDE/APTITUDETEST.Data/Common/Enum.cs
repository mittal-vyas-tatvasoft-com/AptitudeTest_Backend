using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AptitudeTest.Data.Common
{
    public class Enum
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
    }
}
