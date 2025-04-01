using CobolApp.Net.Modules;
using Xunit;

namespace CobolApp.Net.Tests.Modules
{
    /// <summary>
    /// Test class for Calculator module
    /// Migrated from COBOL test_calculator.cbl
    /// </summary>
    public class CalculatorTests
    {
        [Fact]
        public void Add_WithValidNumbers_ReturnsCorrectSum()
        {
            // Test values from COBOL test_calculator.cbl
            decimal num1 = 123.45m;
            decimal num2 = 67.89m;
            decimal expectedResult = 191.34m;

            decimal result = Calculator.Add(num1, num2);

            // Verify result matches expected value
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Add_WithValuesExceedingMaximum_ReturnsMaxValue()
        {
            // Test with values exceeding limits
            decimal largeValue = 1000000.00m;

            // Test upper limit
            decimal result = Calculator.Add(largeValue, 1m);

            // Verify result is limited to MaxValue
            Assert.Equal(Calculator.MaxValue, result);
        }

        [Fact]
        public void Add_WithValuesBelowMinimum_ReturnsMinValue()
        {
            // Test with values below limits
            decimal smallValue = -1000000.00m;

            // Test lower limit
            decimal result = Calculator.Add(smallValue, -1m);

            // Verify result is limited to MinValue
            Assert.Equal(Calculator.MinValue, result);
        }
    }
}
