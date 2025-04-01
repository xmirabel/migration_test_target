package com.app.utils;

import com.app.model.DateModel;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;

/**
 * Utility class for date operations
 * Migrated from COBOL DATE_UTILS module
 */
public class DateUtils {
    
    private static final DateTimeFormatter FORMATTER = DateTimeFormatter.ofPattern("dd/MM/yyyy");
    
    /**
     * Formats a date from DateModel to a string in DD/MM/YYYY format
     * This method replicates the functionality of the COBOL DATE_UTILS program
     * 
     * @param dateModel The date model containing year, month, and day
     * @return A formatted date string in DD/MM/YYYY format
     */
    public static String formatDate(DateModel dateModel) {
        if (dateModel == null) {
            return "";
        }
        
        try {
            // Format the date components with leading zeros if needed
            String day = String.format("%02d", dateModel.getDay());
            String month = String.format("%02d", dateModel.getMonth());
            String year = String.format("%04d", dateModel.getYear());
            
            // Concatenate with slashes, exactly as in the COBOL program
            return day + "/" + month + "/" + year;
        } catch (Exception e) {
            return "";
        }
    }
    
    /**
     * Creates a DateModel from the current date
     * 
     * @return A DateModel representing the current date
     */
    public static DateModel getCurrentDate() {
        LocalDate now = LocalDate.now();
        return new DateModel(now);
    }
    
    /**
     * Parses a date string in DD/MM/YYYY format to a DateModel
     * 
     * @param dateString The date string to parse
     * @return A DateModel representing the parsed date, or null if parsing fails
     */
    public static DateModel parseDate(String dateString) {
        if (dateString == null || dateString.trim().isEmpty()) {
            return null;
        }
        
        try {
            LocalDate date = LocalDate.parse(dateString, FORMATTER);
            return new DateModel(date);
        } catch (Exception e) {
            return null;
        }
    }
}
