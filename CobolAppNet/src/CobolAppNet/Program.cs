using CobolApp.Net.Models;
using CobolApp.Net.Modules;
using CobolApp.Net.Utils;
using Microsoft.Extensions.Logging;

namespace CobolApp.Net
{
    /// <summary>
    /// Main application class
    /// Migrated from COBOL MAIN program
    /// </summary>
    public class Program
    {
        private static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        private static readonly ILogger Logger = LoggerFactory.CreateLogger<Program>();

        public static void Main(string[] args)
        {
            Logger.LogInformation("Application starting");
            Console.WriteLine("=== Application Console .NET Core ===");

            // String utilities demonstration - equivalent to COBOL STRING_UTILS call
            string username = "Utilisateur";
            string greeting = StringUtils.GenerateGreeting(username);
            Console.WriteLine(greeting);

            // Date utilities demonstration - equivalent to COBOL DATE_UTILS call
            DateModel currentDate = DateUtils.GetCurrentDate();
            string formattedDate = DateUtils.FormatDate(currentDate);
            Console.WriteLine($"Date: {formattedDate}");

            // Calculator demonstration - equivalent to COBOL CALCULATOR call
            Console.WriteLine("Entrez le premier nombre:");
            decimal num1;
            try
            {
                if (!decimal.TryParse(Console.ReadLine(), out num1))
                {
                    Logger.LogError("Invalid first number input");
                    num1 = 0;
                }

                // Validate against Calculator constants
                if (num1 > Calculator.MaxValue)
                {
                    Console.WriteLine($"Valeur trop grande, limit�e � {Calculator.MaxValue}");
                    num1 = Calculator.MaxValue;
                }
                else if (num1 < Calculator.MinValue)
                {
                    Console.WriteLine($"Valeur trop petite, limit�e � {Calculator.MinValue}");
                    num1 = Calculator.MinValue;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error processing first number input");
                num1 = 0;
            }

            Console.WriteLine("Entrez le deuxi�me nombre:");
            decimal num2;
            try
            {
                if (!decimal.TryParse(Console.ReadLine(), out num2))
                {
                    Logger.LogError("Invalid second number input");
                    num2 = 0;
                }

                // Validate against Calculator constants
                if (num2 > Calculator.MaxValue)
                {
                    Console.WriteLine($"Valeur trop grande, limit�e � {Calculator.MaxValue}");
                    num2 = Calculator.MaxValue;
                }
                else if (num2 < Calculator.MinValue)
                {
                    Console.WriteLine($"Valeur trop petite, limit�e � {Calculator.MinValue}");
                    num2 = Calculator.MinValue;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error processing second number input");
                num2 = 0;
            }

            decimal result = Calculator.Add(num1, num2);
            Console.WriteLine($"R�sultat de l'addition: {result}");

            // File handler demonstration - equivalent to COBOL FILE_HANDLER call
            string filename = "output.txt";
            string fileContent = $"R�sultat du calcul: {result}";

            int fileStatus = FileHandler.WriteToFile(filename, fileContent);

            if (fileStatus == FileHandler.FileStatusOk)
            {
                Console.WriteLine($"Le r�sultat a �t� enregistr� dans {filename}");
                Logger.LogInformation("Result successfully written to file: {Filename}", filename);
            }
            else if (fileStatus == FileHandler.FileStatusNoname)
            {
                Console.WriteLine("Erreur: Nom de fichier invalide");
                Logger.LogError("Invalid filename: {Filename}", filename);
            }
            else
            {
                Console.WriteLine("Erreur lors de l'enregistrement du fichier");
                Logger.LogError("Error writing to file: {Filename}, status code: {StatusCode}", filename, fileStatus);
            }

            Logger.LogInformation("Application ending");
        }
    }
}