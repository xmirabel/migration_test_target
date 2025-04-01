using CobolApp.Net.Modules;
using System;
using System.IO;
using Xunit;

namespace CobolApp.Net.Tests.Modules
{
    /// <summary>
    /// Test class for FileHandler module
    /// Migrated from COBOL test_file_handler.cbl
    /// </summary>
    public class FileHandlerTests : IDisposable
    {
        private readonly string _testFilePath;

        public FileHandlerTests()
        {
            // Create a temporary file path for testing
            _testFilePath = Path.Combine(Path.GetTempPath(), "test_output.txt");

            // Ensure the file doesn't exist before each test
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        public void Dispose()
        {
            // Clean up after tests
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [Fact]
        public void WriteToFile_WithValidInput_WritesFileSuccessfully()
        {
            // Test data
            string content = "Contenu de test";

            // Test writing to file - equivalent to COBOL FILE_HANDLER call
            int status = FileHandler.WriteToFile(_testFilePath, content);

            // Verify status is OK (0)
            Assert.Equal(FileHandler.FileStatusOk, status);

            // Verify file exists
            Assert.True(File.Exists(_testFilePath));

            // Read content and verify it matches what was written
            string readContent = File.ReadAllText(_testFilePath);
            Assert.Equal(content, readContent);
        }

        [Fact]
        public void WriteToFile_WithNullFilename_ReturnsNoNameStatus()
        {
            // Test with null filename
            int status = FileHandler.WriteToFile(null, "Some content");

            // Verify status is NONAME (2)
            Assert.Equal(FileHandler.FileStatusNoname, status);
        }

        [Fact]
        public void WriteToFile_WithEmptyFilename_ReturnsNoNameStatus()
        {
            // Test with empty filename
            int status = FileHandler.WriteToFile("", "Some content");

            // Verify status is NONAME (2)
            Assert.Equal(FileHandler.FileStatusNoname, status);
        }

        [Fact]
        public void FileExists_WithExistingFile_ReturnsTrue()
        {
            // Create a test file
            File.WriteAllText(_testFilePath, "Test content");

            // Test with existing file
            bool exists = FileHandler.FileExists(_testFilePath);

            // Verify result
            Assert.True(exists);
        }

        [Fact]
        public void FileExists_WithNonExistingFile_ReturnsFalse()
        {
            // Test with non-existing file
            string nonExistingPath = Path.Combine(Path.GetTempPath(), "non_existing_file.txt");
            bool exists = FileHandler.FileExists(nonExistingPath);

            // Verify result
            Assert.False(exists);
        }

        [Fact]
        public void FileExists_WithNullFilename_ReturnsFalse()
        {
            // Test with null filename
            bool exists = FileHandler.FileExists(null);

            // Verify result
            Assert.False(exists);
        }

        [Fact]
        public void FileExists_WithEmptyFilename_ReturnsFalse()
        {
            // Test with empty filename
            bool exists = FileHandler.FileExists("");

            // Verify result
            Assert.False(exists);
        }
    }
}
