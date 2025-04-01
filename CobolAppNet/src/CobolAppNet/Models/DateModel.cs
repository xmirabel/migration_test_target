namespace CobolApp.Net.Models
{
    /// <summary>
    /// Model class for date information
    /// Migrated from COBOL DATE-STRUCTURE in date_utils.cpy
    /// </summary>
    public class DateModel
    {
        /// <summary>
        /// The year (4 digits)
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The month (1-12)
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// The day (1-31)
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DateModel()
        {
        }

        /// <summary>
        /// Constructor with all fields
        /// </summary>
        /// <param name="year">The year (4 digits)</param>
        /// <param name="month">The month (1-12)</param>
        /// <param name="day">The day (1-31)</param>
        public DateModel(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        /// <summary>
        /// Constructor from DateTime
        /// </summary>
        /// <param name="date">DateTime object</param>
        public DateModel(DateTime date)
        {
            Year = date.Year;
            Month = date.Month;
            Day = date.Day;
        }

        /// <summary>
        /// Convert to DateTime
        /// </summary>
        /// <returns>A DateTime representation of this date</returns>
        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day);
        }
    }
}
