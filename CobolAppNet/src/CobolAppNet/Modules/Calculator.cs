namespace CobolApp.Net.Modules
{
    /// <summary>
    /// Calculator module for arithmetic operations
    /// Migrated from COBOL CALCULATOR module
    /// </summary>
    public static class Calculator
    {
        // Constants migrated from calculator.cpy
        public static readonly decimal MaxValue = 999999.99m;
        public static readonly decimal MinValue = -999999.99m;

        /// <summary>
        /// Adds two decimal numbers with precision
        /// This method replicates the functionality of the COBOL CALCULATOR program
        /// </summary>
        /// <param name="num1">First number</param>
        /// <param name="num2">Second number</param>
        /// <returns>The sum of the two numbers, constrained to MinValue and MaxValue</returns>
        public static decimal Add(decimal num1, decimal num2)
        {
            decimal result = num1 + num2;

            // Apply constraints from COBOL constants
            if (result > MaxValue)
            {
                return MaxValue;
            }
            else if (result < MinValue)
            {
                return MinValue;
            }

            return Math.Round(result, 2);
        }
    }
}
