package com.app.utils;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/**
 * Test class for StringUtils
 * Migrated from COBOL test_string_utils.cbl
 */
public class StringUtilsTest {
    
    @Test
    public void testGenerateGreeting() {
        // Test with the same name as in the COBOL test
        // WS-NAME = "Jean"
        String greeting = StringUtils.generateGreeting("Jean");
        
        // Verify the greeting matches the expected value from COBOL test
        // WS-EXPECTED-GREETING = "Bonjour, Jean !"
        assertEquals("Bonjour, Jean !", greeting,
                "Greeting should be correctly formatted");
        
        // Additional tests not in the original COBOL test
        // Test with a name that has whitespace
        greeting = StringUtils.generateGreeting("  Marie  ");
        assertEquals("Bonjour, Marie !", greeting,
                "Greeting should trim whitespace from name");
        
        // Test with null
        greeting = StringUtils.generateGreeting(null);
        assertEquals("Bonjour,  !", greeting,
                "Greeting should handle null input");
        
        // Test with empty string
        greeting = StringUtils.generateGreeting("");
        assertEquals("Bonjour,  !", greeting,
                "Greeting should handle empty input");
    }
    
    @Test
    public void testPad() {
        // Test right padding
        String padded = StringUtils.pad("ABC", 5, false);
        assertEquals("ABC  ", padded,
                "Right padding should add spaces to the right");
        
        // Test left padding
        padded = StringUtils.pad("ABC", 5, true);
        assertEquals("  ABC", padded,
                "Left padding should add spaces to the left");
        
        // Test with custom padding character
        padded = StringUtils.pad("ABC", 5, '0', true);
        assertEquals("00ABC", padded,
                "Left padding with custom character should work");
        
        // Test with string longer than padding length
        padded = StringUtils.pad("ABCDEF", 5, false);
        assertEquals("ABCDEF", padded,
                "Padding should not truncate strings");
        
        // Test with null
        padded = StringUtils.pad(null, 5, false);
        assertEquals("     ", padded,
                "Padding should handle null input");
    }
    
    @Test
    public void testMaxStringLength() {
        // Create a string longer than MAX_STRING_LENGTH
        StringBuilder longString = new StringBuilder();
        for (int i = 0; i < StringUtils.MAX_STRING_LENGTH + 100; i++) {
            longString.append('X');
        }
        
        // Test with a string exceeding MAX_STRING_LENGTH
        String greeting = StringUtils.generateGreeting(longString.toString());
        assertTrue(greeting.length() <= StringUtils.MAX_STRING_LENGTH + 11, // "Bonjour, " + "!" = 11 chars
                "Greeting should limit input to MAX_STRING_LENGTH");
    }
}
