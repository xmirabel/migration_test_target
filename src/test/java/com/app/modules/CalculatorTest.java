package com.app.modules;

import org.junit.jupiter.api.Test;
import java.math.BigDecimal;
import static org.junit.jupiter.api.Assertions.*;

/**
 * Test class for Calculator module
 * Migrated from COBOL test_calculator.cbl
 */
public class CalculatorTest {
    
    @Test
    public void testAdd() {
        // Test values from COBOL test_calculator.cbl
        BigDecimal num1 = new BigDecimal("123.45");
        BigDecimal num2 = new BigDecimal("67.89");
        BigDecimal expectedResult = new BigDecimal("191.34");
        
        BigDecimal result = Calculator.add(num1, num2);
        
        // Verify result matches expected value
        assertEquals(0, expectedResult.compareTo(result), 
                "Addition should return the correct sum");
        
        // Test with null values
        result = Calculator.add(null, num2);
        assertEquals(0, num2.setScale(2, BigDecimal.ROUND_HALF_UP).compareTo(result),
                "Addition with null first parameter should use zero");
        
        result = Calculator.add(num1, null);
        assertEquals(0, num1.setScale(2, BigDecimal.ROUND_HALF_UP).compareTo(result),
                "Addition with null second parameter should use zero");
        
        result = Calculator.add(null, null);
        assertEquals(0, BigDecimal.ZERO.setScale(2, BigDecimal.ROUND_HALF_UP).compareTo(result),
                "Addition with both null parameters should return zero");
    }
    
    @Test
    public void testAddWithLimits() {
        // Test with values exceeding limits
        BigDecimal largeValue = new BigDecimal("1000000.00");
        BigDecimal smallValue = new BigDecimal("-1000000.00");
        
        // Test upper limit
        BigDecimal result = Calculator.add(largeValue, BigDecimal.ONE);
        assertEquals(0, Calculator.MAX_VALUE.compareTo(result),
                "Addition exceeding MAX_VALUE should be limited to MAX_VALUE");
        
        // Test lower limit
        result = Calculator.add(smallValue, BigDecimal.ONE.negate());
        assertEquals(0, Calculator.MIN_VALUE.compareTo(result),
                "Addition below MIN_VALUE should be limited to MIN_VALUE");
    }
}
