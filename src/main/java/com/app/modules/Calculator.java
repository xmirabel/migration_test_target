package com.app.modules;

import java.math.BigDecimal;
import java.math.RoundingMode;

/**
 * Calculator module for arithmetic operations
 * Migrated from COBOL CALCULATOR module
 */
public class Calculator {
    
    // Constants migrated from calculator.cpy
    public static final BigDecimal MAX_VALUE = new BigDecimal("999999.99");
    public static final BigDecimal MIN_VALUE = new BigDecimal("-999999.99");
    
    /**
     * Adds two decimal numbers with precision
     * This method replicates the functionality of the COBOL CALCULATOR program
     * 
     * @param num1 First number
     * @param num2 Second number
     * @return The sum of the two numbers, constrained to MIN_VALUE and MAX_VALUE
     */
    public static BigDecimal add(BigDecimal num1, BigDecimal num2) {
        if (num1 == null) num1 = BigDecimal.ZERO;
        if (num2 == null) num2 = BigDecimal.ZERO;
        
        BigDecimal result = num1.add(num2).setScale(2, RoundingMode.HALF_UP);
        
        // Apply constraints from COBOL constants
        if (result.compareTo(MAX_VALUE) > 0) {
            return MAX_VALUE;
        } else if (result.compareTo(MIN_VALUE) < 0) {
            return MIN_VALUE;
        }
        
        return result;
    }
}
