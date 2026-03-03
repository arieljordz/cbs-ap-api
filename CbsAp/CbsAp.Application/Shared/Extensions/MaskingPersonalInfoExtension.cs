namespace CbsAp.Application.Shared.Extensions
{
    public static class MaskingPersonalInfoExtension
    {
        public static string MaskPersonalInformation(this string input, string maskSymbol = "*", int visibleChars = 4)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            if (input.Contains("@"))
            {
                // Mask email address
                return MaskEmailAddress(input, maskSymbol, visibleChars);
            }
            else
            {
                // Assume it's a phone number and mask it
                return MaskPhoneNumber(input, maskSymbol, visibleChars);
            }
        }

        private static string MaskEmailAddress(string email, string maskSymbol, int visibleChars)
        {
            var parts = email.Split('@');
            if (parts.Length == 2)
            {
                var username = parts[0];
                var domain = parts[1];

                var visibleUsername = MaskVisibleCharacters(username, maskSymbol, visibleChars);
                var visibleDomain = MaskVisibleCharacters(domain, maskSymbol, 0);

                return $"{visibleUsername}@{visibleDomain}";
            }

            return email;
        }

        private static string MaskPhoneNumber(string phoneNumber, string maskSymbol, int visibleChars)
        {
            if (phoneNumber.Length >= visibleChars)
            {
                var visiblePart = phoneNumber.Substring(phoneNumber.Length - visibleChars);
                var maskedPart = MaskVisibleCharacters(phoneNumber.Substring(0, phoneNumber.Length - visibleChars), maskSymbol, visibleChars);

                return $"{maskedPart}{visiblePart}";
            }

            return phoneNumber;
        }

        private static string MaskVisibleCharacters(string input, string maskSymbol, int visibleChars)
        {
            if (input.Length >= visibleChars)
            {
                return new string(maskSymbol[0], input.Length - visibleChars) + input.Substring(input.Length - visibleChars);
            }

            return new string(maskSymbol[0], input.Length);
        }
    }
}