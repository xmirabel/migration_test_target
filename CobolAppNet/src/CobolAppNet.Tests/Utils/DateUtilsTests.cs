using CobolApp.Net.Models;
using CobolApp.Net.Utils;
using System;
using Xunit;

namespace CobolApp.Net.Tests.Utils
{
    /// <summary>
    /// Test class for DateUtils
    /// Migrated from COBOL test_date_utils.cbl
    /// </summary>
    public class DateUtilsTests
    {
        [Fact]
        public void FormatDate_WithValidDate_ReturnsFormattedString()
        {
            // Create a test date model with the same values as in the COBOL test
            // WS-DATE with WS-YEAR=2023, WS-MONTH=05, WS-DAY=15
            DateModel dateModel = new DateModel(2023, 5, 15);

            // Format the date
            string formattedDate = DateUtils.FormatDate(dateModel);

            // Verify the formatted date matches the expected value from COBOL test
            // WS-EXPECTED-DATE = "15/05/2023"
            Assert.Equal("15/05/2023", formattedDate);
        }

        [Fact]
        public void FormatDate_WithNullDate_ReturnsEmptyString()
        {
            // Test with null
            string formattedDate = DateUtils.FormatDate(null);

            // Verify result
            Assert.Equal(string.Empty, formattedDate);
        }

        [Fact]
        public void GetCurrentDate_ReturnsValidDate()
        {
            // Get current date
            DateModel currentDate = DateUtils.GetCurrentDate();

            // Verify the date is valid
            Assert.NotNull(currentDate);
            Assert.True(currentDate.Year >= 2023);
            Assert.True(currentDate.Month >= 1 && currentDate.Month <= 12);
            Assert.True(currentDate.Day >= 1 && currentDate.Day <= 31);
        }

        [Fact]
        public void ParseDate_WithValidDateString_ReturnsDateModel()
        {
            // Parse a date string
            DateModel? dateModel = DateUtils.ParseDate("15/05/2023");

            // Verify the parsed date
            Assert.NotNull(dateModel);
            Assert.Equal(2023, dateModel.Year);
            Assert.Equal(5, dateModel.Month);
            Assert.Equal(15, dateModel.Day);
        }

        [Fact]
        public void ParseDate_WithInvalidDateString_ReturnsNull()
        {
            // Test with invalid date string
            DateModel? dateModel = DateUtils.ParseDate("invalid-date");

            // Verify result
            Assert.Null(dateModel);
        }

        [Fact]
        public void ParseDate_WithNullString_ReturnsNull()
        {
            // Test with null string
            DateModel? dateModel = DateUtils.ParseDate(null);

            // Verify result
            Assert.Null(dateModel);
        }

        [Fact]
        public void ParseDate_WithEmptyString_ReturnsNull()
        {
            // Test with empty string
            DateModel? dateModel = DateUtils.ParseDate("");

            // Verify result
            Assert.Null(dateModel);
        }
    }
}