using CobolApp.Net.Utils;
using Xunit;

namespace CobolApp.Net.Tests.Utils
{
    /// <summary>
    /// Test class for StringUtils
    /// Migrated from COBOL test_string_utils.cbl
    /// </summary>
    public class StringUtilsTests
    {
        [Fact]
        public void GenerateGreeting_WithValidName_ReturnsCorrectGreeting()
        {
            // Test with the same name as in the COBOL test
            // WS-NAME = "Jean"
            string greeting = StringUtils.GenerateGreeting("Jean");

            // Verify the greeting matches the expected value from COBOL test
            // WS-EXPECTED-GREETING = "Bonjour, Jean !"
            Assert.Equal("Bonjour, Jean !", greeting);
        }

        [Fact]
        public void GenerateGreeting_WithNameContainingWhitespace_TrimsName()
        {
            // Test with a name that has whitespace
            string greeting = StringUtils.GenerateGreeting("  Marie  ");

            // Verify the greeting has trimmed the name
            Assert.Equal("Bonjour, Marie !", greeting);
        }

        [Fact]
        public void GenerateGreeting_WithNullName_HandlesNull()
        {
            // Test with null
            string greeting = StringUtils.GenerateGreeting(null);

            // Verify the greeting handles null
            Assert.Equal("Bonjour,  !", greeting);
        }

        [Fact]
        public void GenerateGreeting_WithEmptyName_HandlesEmpty()
        {
            // Test with empty string
            string greeting = StringUtils.GenerateGreeting("");

            // Verify the greeting handles empty string
            Assert.Equal("Bonjour,  !", greeting);
        }

        [Fact]
        public void Pad_WithRightPadding_AddsSpacesToRight()
        {
            // Test right padding
            string padded = StringUtils.Pad("ABC", 5, false);

            // Verify result
            Assert.Equal("ABC  ", padded);
        }

        [Fact]
        public void Pad_WithLeftPadding_AddsSpacesToLeft()
        {
            // Test left padding
            string padded = StringUtils.Pad("ABC", 5, true);

            // Verify result
            Assert.Equal("  ABC", padded);
        }

        [Fact]
        public void Pad_WithCustomPaddingChar_UsesThatChar()
        {
            // Test with custom padding character
            string padded = StringUtils.Pad("ABC", 5, '0', true);

            // Verify result
            Assert.Equal("00ABC", padded);
        }

        [Fact]
        public void Pad_WithStringLongerThanLength_DoesNotTruncate()
        {
            // Test with string longer than padding length
            string padded = StringUtils.Pad("ABCDEF", 5, false);

            // Verify result
            Assert.Equal("ABCDEF", padded);
        }

        [Fact]
        public void Pad_WithNullInput_HandlesNull()
        {
            // Test with null
            string padded = StringUtils.Pad(null, 5, false);

            // Verify result
            Assert.Equal("     ", padded);
        }

        [Fact]
        public void GenerateGreeting_WithVeryLongName_LimitsLength()
        {
            // Create a string longer than MaxStringLength
            string longName = new string('X', StringUtils.MaxStringLength + 100);

            // Test with a string exceeding MaxStringLength
            string greeting = StringUtils.GenerateGreeting(longName);

            // Verify the name was limited
            Assert.True(greeting.Length <= StringUtils.MaxStringLength + 11); // "Bonjour, " + "!" = 11 chars
        }
    }
}