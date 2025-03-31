package com.app;

import com.app.model.DateModel;
import com.app.modules.Calculator;
import com.app.modules.FileHandler;
import com.app.utils.DateUtils;
import com.app.utils.StringUtils;

import java.math.BigDecimal;
import java.util.Scanner;
import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

/**
 * Main application class
 * Migrated from COBOL MAIN program
 */
public class Main {
    
    private static final Logger logger = LogManager.getLogger(Main.class);
    
    public static void main(String[] args) {
        logger.info("Application starting");
        System.out.println("=== Application Console Java ===");
        
        Scanner scanner = new Scanner(System.in);
        
        // String utilities demonstration - equivalent to COBOL STRING_UTILS call
        String username = "Utilisateur";
        String greeting = StringUtils.generateGreeting(username);
        System.out.println(greeting);
        
        // Date utilities demonstration - equivalent to COBOL DATE_UTILS call
        DateModel currentDate = DateUtils.getCurrentDate();
        String formattedDate = DateUtils.formatDate(currentDate);
        System.out.println("Date: " + formattedDate);
        
        // Calculator demonstration - equivalent to COBOL CALCULATOR call
        System.out.println("Entrez le premier nombre:");
        BigDecimal num1;
        try {
            num1 = new BigDecimal(scanner.nextLine());
            // Validate against Calculator constants
            if (num1.compareTo(Calculator.MAX_VALUE) > 0) {
                System.out.println("Valeur trop grande, limitée à " + Calculator.MAX_VALUE);
                num1 = Calculator.MAX_VALUE;
            } else if (num1.compareTo(Calculator.MIN_VALUE) < 0) {
                System.out.println("Valeur trop petite, limitée à " + Calculator.MIN_VALUE);
                num1 = Calculator.MIN_VALUE;
            }
        } catch (NumberFormatException e) {
            logger.error("Invalid first number input", e);
            num1 = BigDecimal.ZERO;
        }
        
        System.out.println("Entrez le deuxième nombre:");
        BigDecimal num2;
        try {
            num2 = new BigDecimal(scanner.nextLine());
            // Validate against Calculator constants
            if (num2.compareTo(Calculator.MAX_VALUE) > 0) {
                System.out.println("Valeur trop grande, limitée à " + Calculator.MAX_VALUE);
                num2 = Calculator.MAX_VALUE;
            } else if (num2.compareTo(Calculator.MIN_VALUE) < 0) {
                System.out.println("Valeur trop petite, limitée à " + Calculator.MIN_VALUE);
                num2 = Calculator.MIN_VALUE;
            }
        } catch (NumberFormatException e) {
            logger.error("Invalid second number input", e);
            num2 = BigDecimal.ZERO;
        }
        
        BigDecimal result = Calculator.add(num1, num2);
        System.out.println("Résultat de l'addition: " + result);
        
        // File handler demonstration - equivalent to COBOL FILE_HANDLER call
        String filename = "output.txt";
        String fileContent = "Résultat du calcul: " + result;
        
        int fileStatus = FileHandler.writeToFile(filename, fileContent);
        
        if (fileStatus == FileHandler.FILE_STATUS_OK) {
            System.out.println("Le résultat a été enregistré dans " + filename);
            logger.info("Result successfully written to file: {}", filename);
        } else if (fileStatus == FileHandler.FILE_STATUS_NONAME) {
            System.out.println("Erreur: Nom de fichier invalide");
            logger.error("Invalid filename: {}", filename);
        } else {
            System.out.println("Erreur lors de l'enregistrement du fichier");
            logger.error("Error writing to file: {}, status code: {}", filename, fileStatus);
        }
        
        scanner.close();
        logger.info("Application ending");
    }
}