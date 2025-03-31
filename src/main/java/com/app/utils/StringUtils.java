package com.app.utils;

/**
 * Utility class for string operations
 * Migrated from COBOL STRING_UTILS module
 */
public class StringUtils {
    
    // Constants migrated from string_utils.cpy
    public static final int MAX_STRING_LENGTH = 1000;
    public static final char DEFAULT_PADDING = ' ';
    
    /**
     * Generates a greeting message for the given username
     * This method replicates the functionality of the COBOL STRING_UTILS program
     * 
     * @param username The name of the user to greet
     * @return A personalized greeting message
     */
    public static String generateGreeting(String username) {
        if (username == null) {
            username = "";
        }
        
        // Ensure the username doesn't exceed the maximum length
        if (username.length() > MAX_STRING_LENGTH) {
            username = username.substring(0, MAX_STRING_LENGTH);
        }
        
        // Trim the username to match COBOL's DELIMITED BY SPACE behavior
        String trimmedName = username.trim();
        
        // Concatenate the greeting parts, exactly as in the COBOL program
        return "Bonjour, " + trimmedName + " !";
    }
    
    /**
     * Pads a string to the specified length with the default padding character
     * 
     * @param input The input string to pad
     * @param length The desired length
     * @param padLeft If true, pad on the left; otherwise pad on the right
     * @return The padded string
     */
    public static String pad(String input, int length, boolean padLeft) {
        return pad(input, length, DEFAULT_PADDING, padLeft);
    }
    
    /**
     * Pads a string to the specified length with the given padding character
     * 
     * @param input The input string to pad
     * @param length The desired length
     * @param padChar The character to use for padding
     * @param padLeft If true, pad on the left; otherwise pad on the right
     * @return The padded string
     */
    public static String pad(String input, int length, char padChar, boolean padLeft) {
        if (input == null) {
            input = "";
        }
        
        if (input.length() >= length) {
            return input;
        }
        
        StringBuilder sb = new StringBuilder();
        int padLength = length - input.length();
        
        if (padLeft) {
            for (int i = 0; i < padLength; i++) {
                sb.append(padChar);
            }
            sb.append(input);
        } else {
            sb.append(input);
            for (int i = 0; i < padLength; i++) {
                sb.append(padChar);
            }
        }
        
        return sb.toString();
    }
}
