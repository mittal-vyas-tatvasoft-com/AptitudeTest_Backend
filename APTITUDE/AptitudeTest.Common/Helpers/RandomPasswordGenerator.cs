using System.Text;

namespace AptitudeTest.Common.Helpers
{
    public class RandomPasswordGenerator
    {
        private static readonly string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string specialChars = "!@#$%^&*()_+-=[]{}|;:'\"<>,.?/";
        private static readonly string digitChars = "0123456789";

        private static Random random = new Random();

        public static string GenerateRandomPassword(int length, bool includeDigits = true)
        {
            if (length < 8)
            {
                throw new ArgumentException("Password length must be at least 8 characters.");
            }

            StringBuilder password = new StringBuilder();

            // Ensure at least one lowercase, one uppercase, and one special character
            password.Append(GetRandomCharacter(lowercaseChars));
            password.Append(GetRandomCharacter(uppercaseChars));
            password.Append(GetRandomCharacter(specialChars));

            if (includeDigits)
            {
                password.Append(GetRandomCharacter(digitChars));
            }

            // Generate the remaining characters
            for (int i = 4; i < length; i++)
            {
                string charSet = lowercaseChars + uppercaseChars + (includeDigits ? digitChars : "") + specialChars;
                password.Append(GetRandomCharacter(charSet));
            }

            // Shuffle the characters to make it more random
            password = ShuffleString(password);

            return password.ToString();
        }

        private static char GetRandomCharacter(string charSet)
        {
            int index = random.Next(charSet.Length);
            return charSet[index];
        }

        private static StringBuilder ShuffleString(StringBuilder str)
        {
            for (int i = str.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                char temp = str[i];
                str[i] = str[j];
                str[j] = temp;
            }
            return str;
        }


    }
}
