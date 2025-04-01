package com.app.modules;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

/**
 * File handler module for file operations
 * Migrated from COBOL FILE_HANDLER module
 */
public class FileHandler {
    
    // Constants migrated from file_handler.cpy
    public static final int FILE_STATUS_OK = 0;
    public static final int FILE_STATUS_ERROR = 1;
    public static final int FILE_STATUS_NONAME = 2;
    
    /**
     * Writes content to a file
     * This method replicates the functionality of the COBOL FILE_HANDLER program
     * 
     * @param filename The name of the file to write to
     * @param content The content to write to the file
     * @return Status code (0=success, 1=error, 2=no filename)
     */
    public static int writeToFile(String filename, String content) {
        if (filename == null || filename.trim().isEmpty()) {
            return FILE_STATUS_NONAME;
        }
        
        try (BufferedWriter writer = new BufferedWriter(new FileWriter(filename))) {
            writer.write(content);
            return FILE_STATUS_OK;
        } catch (IOException e) {
            return FILE_STATUS_ERROR;
        }
    }
    
    /**
     * Checks if a file exists
     * 
     * @param filename The name of the file to check
     * @return true if the file exists, false otherwise
     */
    public static boolean fileExists(String filename) {
        if (filename == null || filename.trim().isEmpty()) {
            return false;
        }
        
        Path path = Paths.get(filename);
        return Files.exists(path);
    }
}
