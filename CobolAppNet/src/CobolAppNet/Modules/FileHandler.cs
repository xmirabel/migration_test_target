namespace CobolApp.Net.Modules
{
    /// <summary>
    /// File handler module for file operations
    /// Migrated from COBOL FILE_HANDLER module
    /// </summary>
    public static class FileHandler
    {
        // Constants migrated from file_handler.cpy
        public const int FileStatusOk = 0;
        public const int FileStatusError = 1;
        public const int FileStatusNoname = 2;

        /// <summary>
        /// Writes content to a file
        /// This method replicates the functionality of the COBOL FILE_HANDLER program
        /// </summary>
        /// <param name="filename">The name of the file to write to</param>
        /// <param name="content">The content to write to the file</param>
        /// <returns>Status code (0=success, 1=error, 2=no filename)</returns>
        public static int WriteToFile(string? filename, string? content)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return FileStatusNoname;
            }

            try
            {
                File.WriteAllText(filename, content ?? string.Empty);
                return FileStatusOk;
            }
            catch (Exception)
            {
                return FileStatusError;
            }
        }

        /// <summary>
        /// Checks if a file exists
        /// </summary>
        /// <param name="filename">The name of the file to check</param>
        /// <returns>true if the file exists, false otherwise</returns>
        public static bool FileExists(string? filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return false;
            }

            return File.Exists(filename);
        }
    }
}