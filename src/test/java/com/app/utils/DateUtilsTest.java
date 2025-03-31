package com.app.utils;

import com.app.model.DateModel;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/**
 * Test class for DateUtils
 * Migrated from COBOL test_date_utils.cbl
 */
public class DateUtilsTest {
    
    @Test
    public void testFormatDate() {
        // Create a test date model with the same values as in the COBOL test
        // WS-DATE with WS-YEAR=2023, WS-MONTH=05, WS-DAY=15
        DateModel dateModel = new DateModel(2023, 5, 15);
        
        // Format the date
        String formattedDate = DateUtils.formatDate(dateModel);
        
        // Verify the formatted date matches the expected value from COBOL test
        // WS-EXPECTED-DATE = "15/05/2023"
        assertEquals("15/05/2023", formattedDate,
                "Date should be formatted as DD/MM/YYYY");
        
        // Additional tests not in the original COBOL test
        // Test with null
        assertNotNull(DateUtils.formatDate(null),
                "formatDate should handle null input");
        assertEquals("", DateUtils.formatDate(null),
                "formatDate should return empty string for null input");
    }
    
    @Test
    public void testGetCurrentDate() {
        // Get current date
        DateModel currentDate = DateUtils.getCurrentDate();
        
        // Verify the date is not null and has reasonable values
        assertNotNull(currentDate, "getCurrentDate should not return null");
        assertTrue(currentDate.getYear() >= 2023,
                "Year should be at least 2023");
        assertTrue(currentDate.getMonth() >= 1 && currentDate.getMonth() <= 12,
                "Month should be between 1 and 12");
        assertTrue(currentDate.getDay() >= 1 && currentDate.getDay() <= 31,
                "Day should be between 1 and 31");
    }
    
    @Test
    public void testParseDate() {
        // Parse a date string
        DateModel dateModel = DateUtils.parseDate("15/05/2023");
        
        // Verify the parsed date
        assertNotNull(dateModel, "parseDate should not return null for valid input");
        assertEquals(2023, dateModel.getYear(), "Year should be parsed correctly");
        assertEquals(5, dateModel.getMonth(), "Month should be parsed correctly");
        assertEquals(15, dateModel.getDay(), "Day should be parsed correctly");
        
        // Test with invalid input
        assertNull(DateUtils.parseDate("invalid-date"),
                "parseDate should return null for invalid input");
        assertNull(DateUtils.parseDate(null),
                "parseDate should return null for null input");
        assertNull(DateUtils.parseDate(""),
                "parseDate should return null for empty input");
    }
}
