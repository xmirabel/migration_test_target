namespace CobolApp.Net.Utils
{
    /// <summary>
    /// Utility class for string operations
    /// Migrated from COBOL STRING_UTILS module
    /// </summary>
    public static class StringUtils
    {
        // Constants migrated from string_utils.cpy
        public const int MaxStringLength = 1000;
        public const char DefaultPadding = ' ';

        /// <summary>
        /// Generates a greeting message for the given username
        /// This method replicates the functionality of the COBOL STRING_UTILS program
        /// </summary>
        /// <param name="username">The name of the user to greet</param>
        /// <returns>A personalized greeting message</returns>
        public static string GenerateGreeting(string? username)
        {
            if (username == null)
            {
                username = string.Empty;
            }

            // Ensure the username doesn't exceed the maximum length
            if (username.Length > MaxStringLength)
            {
                username = username.Substring(0, MaxStringLength);
            }

            // Trim the username to match COBOL's DELIMITED BY SPACE behavior
            string trimmedName = username.Trim();

            // Concatenate the greeting parts, exactly as in the COBOL program
            return $"Bonjour, {trimmedName} !";
        }

        /// <summary>
        /// Pads a string to the specified length with the default padding character
        /// </summary>
        /// <param name="input">The input string to pad</param>
        /// <param name="length">The desired length</param>
        /// <param name="padLeft">If true, pad on the left; otherwise pad on the right</param>
        /// <returns>The padded string</returns>
        public static string Pad(string? input, int length, bool padLeft)
        {
            return Pad(input, length, DefaultPadding, padLeft);
        }

        /// <summary>
        /// Pads a string to the specified length with the given padding character
        /// </summary>
        /// <param name="input">The input string to pad</param>
        /// <param name="length">The desired length</param>
        /// <param name="padChar">The character to use for padding</param>
        /// <param name="padLeft">If true, pad on the left; otherwise pad on the right</param>
        /// <returns>The padded string</returns>
        public static string Pad(string? input, int length, char padChar, bool padLeft)
        {
            if (input == null)
            {
                input = string.Empty;
            }

            if (input.Length >= length)
            {
                return input;
            }

            if (padLeft)
            {
                return input.PadLeft(length, padChar);
            }
            else
            {
                return input.PadRight(length, padChar);
            }
        }
    }
}
