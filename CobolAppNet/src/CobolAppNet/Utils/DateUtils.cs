using CobolApp.Net.Models;

namespace CobolApp.Net.Utils
{
    /// <summary>
    /// Utility class for date operations
    /// Migrated from COBOL DATE_UTILS module
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// Formats a date from DateModel to a string in DD/MM/YYYY format
        /// This method replicates the functionality of the COBOL DATE_UTILS program
        /// </summary>
        /// <param name="dateModel">The date model containing year, month, and day</param>
        /// <returns>A formatted date string in DD/MM/YYYY format</returns>
        public static string FormatDate(DateModel? dateModel)
        {
            if (dateModel == null)
            {
                return string.Empty;
            }

            try
            {
                // Format the date components with leading zeros if needed
                string day = dateModel.Day.ToString("00");
                string month = dateModel.Month.ToString("00");
                string year = dateModel.Year.ToString("0000");

                // Concatenate with slashes, exactly as in the COBOL program
                return $"{day}/{month}/{year}";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Creates a DateModel from the current date
        /// </summary>
        /// <returns>A DateModel representing the current date</returns>
        public static DateModel GetCurrentDate()
        {
            DateTime now = DateTime.Now;
            return new DateModel(now);
        }

        /// <summary>
        /// Parses a date string in DD/MM/YYYY format to a DateModel
        /// </summary>
        /// <param name="dateString">The date string to parse</param>
        /// <returns>A DateModel representing the parsed date, or null if parsing fails</returns>
        public static DateModel? ParseDate(string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return null;
            }

            try
            {
                // Try to parse the date string in DD/MM/YYYY format
                if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    return new DateModel(date);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
