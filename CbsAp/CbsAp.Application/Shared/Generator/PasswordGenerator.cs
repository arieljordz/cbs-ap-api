using CbsAp.Application.Abstractions.Shared;
using CbsAp.Application.DTOs.Shared;
using System.Security.Cryptography;
using System.Text;

namespace CbsAp.Application.Shared.Generator
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private const string _uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string _lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string _digits = "0123456789";
        private const string _symbols = "!@#$%^*_";

        public string Generate(PasswordOptions options)
        {
            if (options.Length < 8)
                throw new ArgumentException("Password length must be at least 8 characters.");

            var requiredChars = new List<char>();
            var characterPool = new StringBuilder();

            // Add one char from each selected category to required list and pool
            if (options.IncludeUppercase)
            {
                requiredChars.Add(GetRandomChar(_uppercase));
                characterPool.Append(_uppercase);
            }

            if (options.IncludeLowercase)
            {
                requiredChars.Add(GetRandomChar(_lowercase));
                characterPool.Append(_lowercase);
            }

            if (options.IncludeDigits)
            {
                requiredChars.Add(GetRandomChar(_digits));
                characterPool.Append(_digits);
            }

            if (options.IncludeSymbols)
            {
                requiredChars.Add(GetRandomChar(_symbols));
                characterPool.Append(_symbols);
            }

            if (characterPool.Length == 0)
                throw new ArgumentException("At least one character set must be selected.");

            var remainingLength = options.Length - requiredChars.Count;
            var passwordChars = new List<char>(options.Length);
            passwordChars.AddRange(requiredChars);

            // Fill the rest with random characters from the total pool
            for (int i = 0; i < remainingLength; i++)
            {
                passwordChars.Add(GetRandomChar(characterPool.ToString()));
            }

            // Shuffle the final password to avoid predictable patterns
            Shuffle(passwordChars);

            return new string([.. passwordChars]);
        }

        private static char GetRandomChar(string chars)
        {
            int index = RandomNumberGenerator.GetInt32(chars.Length);
            return chars[index];
        }

        private static void Shuffle(List<char> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
