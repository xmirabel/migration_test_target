package com.app.model;

import java.time.LocalDate;

/**
 * Model class for date information
 * Migrated from COBOL DATE-STRUCTURE in date_utils.cpy
 */
public class DateModel {
    // Fields corresponding to COBOL DATE-STRUCTURE
    private int year;    // DATE-YEAR
    private int month;   // DATE-MONTH
    private int day;     // DATE-DAY
    
    /**
     * Default constructor
     */
    public DateModel() {
        // Default constructor
    }
    
    /**
     * Constructor with all fields
     * 
     * @param year The year (4 digits)
     * @param month The month (1-12)
     * @param day The day (1-31)
     */
    public DateModel(int year, int month, int day) {
        this.year = year;
        this.month = month;
        this.day = day;
    }
    
    /**
     * Constructor from LocalDate
     * 
     * @param date Java LocalDate object
     */
    public DateModel(LocalDate date) {
        this.year = date.getYear();
        this.month = date.getMonthValue();
        this.day = date.getDayOfMonth();
    }
    
    /**
     * Get the year
     * 
     * @return The year value
     */
    public int getYear() {
        return year;
    }
    
    /**
     * Set the year
     * 
     * @param year The year value to set
     */
    public void setYear(int year) {
        this.year = year;
    }
    
    /**
     * Get the month
     * 
     * @return The month value (1-12)
     */
    public int getMonth() {
        return month;
    }
    
    /**
     * Set the month
     * 
     * @param month The month value to set (1-12)
     */
    public void setMonth(int month) {
        this.month = month;
    }
    
    /**
     * Get the day
     * 
     * @return The day value (1-31)
     */
    public int getDay() {
        return day;
    }
    
    /**
     * Set the day
     * 
     * @param day The day value to set (1-31)
     */
    public void setDay(int day) {
        this.day = day;
    }
    
    /**
     * Convert to Java LocalDate
     * 
     * @return A LocalDate representation of this date
     */
    public LocalDate toLocalDate() {
        return LocalDate.of(year, month, day);
    }
}
