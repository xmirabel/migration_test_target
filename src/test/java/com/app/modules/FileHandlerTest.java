package com.app.modules;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;
import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import static org.junit.jupiter.api.Assertions.*;

/**
 * Test class for FileHandler module
 * Migrated from COBOL test_file_handler.cbl
 */
public class FileHandlerTest {
    
    @TempDir
    Path tempDir;
    
    @Test
    public void testWriteToFile() throws IOException {
        // Create a temporary file path
        Path tempFile = tempDir.resolve("test_output.txt");
        String filename = tempFile.toString();
        String content = "Contenu de test";
        
        // Test writing to file - equivalent to COBOL FILE_HANDLER call
        int status = FileHandler.writeToFile(filename, content);
        
        // Verify status is OK (0)
        assertEquals(FileHandler.FILE_STATUS_OK, status, 
                "File write should return OK status");
        
        // Verify file exists
        assertTrue(Files.exists(tempFile), 
                "File should exist after writing");
        
        // Read content and verify it matches what was written
        String readContent = Files.readString(tempFile);
        assertEquals(content, readContent, 
                "File content should match what was written");
    }
    
    @Test
    public void testWriteToFileWithInvalidFilename() {
        // Test with null filename
        int status = FileHandler.writeToFile(null, "Some content");
        assertEquals(FileHandler.FILE_STATUS_NONAME, status,
                "Writing with null filename should return NONAME status");
        
        // Test with empty filename
        status = FileHandler.writeToFile("", "Some content");
        assertEquals(FileHandler.FILE_STATUS_NONAME, status,
                "Writing with empty filename should return NONAME status");
    }
    
    @Test
    public void testFileExists() throws IOException {
        // Create a temporary file
        Path tempFile = tempDir.resolve("existing_file.txt");
        Files.writeString(tempFile, "Test content");
        
        // Test with existing file
        assertTrue(FileHandler.fileExists(tempFile.toString()),
                "fileExists should return true for existing file");
        
        // Test with non-existing file
        assertFalse(FileHandler.fileExists(tempDir.resolve("non_existing_file.txt").toString()),
                "fileExists should return false for non-existing file");
        
        // Test with null and empty filename
        assertFalse(FileHandler.fileExists(null),
                "fileExists should return false for null filename");
        assertFalse(FileHandler.fileExists(""),
                "fileExists should return false for empty filename");
    }
}
