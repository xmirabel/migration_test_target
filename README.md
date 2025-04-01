# Java Console Application

This project is a console application developed in Java that demonstrates the use of reusable modules and a modular architecture. The application allows performing simple operations such as date formatting, string manipulation, arithmetic calculations, and file writing.

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Compilation](#compilation)
- [Execution](#execution)
- [Tests](#tests)
- [Project Structure](#project-structure)
- [Diagram Generation](#diagram-generation)
- [Module Details](#module-details)
- [Test Details](#test-details)
- [Java Development Best Practices](#java-development-best-practices)
- [Integration with Other Systems](#integration-with-other-systems)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

## Features

The application offers the following features:

- **Date Formatting**: Converting a date from object format to DD/MM/YYYY format
- **String Manipulation**: Generating personalized greeting messages
- **Arithmetic Calculations**: Adding two numbers
- **File Management**: Writing results to a text file

## Architecture

The application is structured according to a modular architecture, with reusable components organized into modules and utilities.

### Component Diagram

![Component Diagram](./docs/images/component-diagram.png)

The application is divided into several components:
- **Main Program**: Coordinates the use of different modules
- **Utility Modules**: Provide basic functionalities (DateUtils, StringUtils)
- **Functional Modules**: Implement business functionalities (Calculator, FileHandler)
- **Data Models**: Represent data structures used (DateModel)

### Structure Diagram

![Structure Diagram](./docs/images/structure-diagram.png)

## Prerequisites

- Java JDK 11 or higher
- Maven 3.6 or higher

## Installation

### Installing Dependencies on Ubuntu/Debian

```bash
sudo apt update
sudo apt install -y openjdk-11-jdk maven
```

### Installing Dependencies on Windows

1. Download and install Java JDK from [Oracle's official website](https://www.oracle.com/java/technologies/javase-jdk11-downloads.html)
2. Download and install Maven from [Apache Maven's official website](https://maven.apache.org/download.cgi)
3. Configure JAVA_HOME and PATH environment variables

## Compilation

The project uses Maven as a build system. To compile the project:

```bash
# Compile the project
mvn clean compile

# Create an executable package
mvn clean package
```

## Execution

After compilation, you can run the application in several ways:

### Execution with Maven

```bash
mvn exec:java -Dexec.mainClass="com.app.Main"
```

### Execution of the JAR

```bash
java -jar target/java-app-1.0-SNAPSHOT-jar-with-dependencies.jar
```

## Tests

The project includes a suite of unit tests to verify the proper functioning of the modules.

### Running All Tests

```bash
mvn test
```

### Running a Specific Test

```bash
mvn test -Dtest=DateUtilsTest
mvn test -Dtest=StringUtilsTest
mvn test -Dtest=CalculatorTest
mvn test -Dtest=FileHandlerTest
```

### Test Report

Maven automatically generates test reports in the `target/surefire-reports` directory.

## Project Structure

```
java-app/
├── pom.xml                     # Maven build file
├── src/
│   ├── main/
│   │   ├── java/
│   │   │   └── com/
│   │   │       └── app/
│   │   │           ├── Main.java              # Main application
│   │   │           ├── model/
│   │   │           │   └── DateModel.java     # Date model
│   │   │           ├── modules/
│   │   │           │   ├── Calculator.java    # Calculator module
│   │   │           │   └── FileHandler.java   # File handler module
│   │   │           └── utils/
│   │   │               ├── DateUtils.java     # Date utilities
│   │   │               └── StringUtils.java   # String utilities
│   │   └── resources/
│   │       └── log4j2.xml                     # Logging configuration
│   └── test/
│       ├── java/
│       │   └── com/
│       │       └── app/
│       │           ├── modules/
│       │           │   ├── CalculatorTest.java
│       │           │   └── FileHandlerTest.java
│       │           └── utils/
│       │               ├── DateUtilsTest.java
│       │               └── StringUtilsTest.java
│       └── resources/
└── docs/
    ├── images/                                # Generated diagrams
    └── puml/                                  # PlantUML source files
```

## Diagram Generation

To generate images from PlantUML files, you can use the following command:

```bash
# Create directory for images
mkdir -p docs/images

# Generate images from PlantUML files
for file in docs/puml/*.puml; do
  output_file="docs/images/$(basename ${file%.puml}).png"
  plantuml -tpng $file -o ../images
done
```

You will need to install PlantUML to run this command:

```bash
# On Ubuntu/Debian
sudo apt install -y plantuml

# Or with Java
# Download plantuml.jar from http://plantuml.com/download
# Then run:
# java -jar plantuml.jar docs/puml/*.puml -o docs/images
```

## Module Details

### DateUtils Module

This module formats a date from object format to DD/MM/YYYY format. It takes a date model containing the year, month, and day as input, and returns a formatted string.

### StringUtils Module

This module generates a personalized greeting message. It takes a username as input and returns a complete greeting message.

### Calculator Module

This module performs a simple addition between two numbers. It takes two decimal numbers as input and returns their sum.

### FileHandler Module

This module handles writing data to a file. It takes a filename and content as input, writes the content to the specified file, and returns a status code indicating whether the operation was successful.

## Test Details

### DateUtils Test

This test verifies that the DateUtils module correctly formats a date. It defines a test date (15/05/2023) and verifies that the formatting result matches the expected value.

### StringUtils Test

This test verifies that the StringUtils module correctly generates a greeting message. It defines a username ("Jean") and verifies that the generated message matches the expected value ("Bonjour, Jean !").

### Calculator Test

This test verifies that the Calculator module correctly adds two numbers. It defines two numbers (123.45 and 67.89) and verifies that their calculated sum matches the expected value (191.34).

### FileHandler Test

This test verifies that the FileHandler module correctly writes content to a file. It defines a filename and content, calls the module to write the file, then verifies that the file was created and its content matches the expected value.

## Java Development Best Practices

This project follows several Java development best practices:

1. **Modularity**: The code is organized into reusable modules with well-defined responsibilities.
2. **Unit Testing**: Each module has unit tests to verify its proper functioning.
3. **Error Handling**: Modules return status codes or exceptions to indicate success or failure of operations.
4. **Documentation**: The code is well-documented with JavaDoc comments.
5. **Consistent Naming**: Variables and modules follow a consistent naming convention (camelCase for methods and variables, PascalCase for classes).
6. **Separation of Concerns**: Functionalities are separated into distinct modules according to their responsibility.
7. **Immutability**: Objects are designed to be immutable when possible.
8. **Resource Management**: Use of try-with-resources to ensure resource closure.

## Integration with Other Systems

This application can be integrated with other systems in several ways:

1. **REST API**: The application can be extended to expose its functionalities via a REST API using Spring Boot.
2. **Databases**: The application can be connected to databases via JDBC or JPA.
3. **Messaging**: The application can be integrated with messaging systems like Apache Kafka or RabbitMQ.
4. **Web Services**: The application can consume or expose SOAP or REST web services.
5. **Microservices**: The modules can be transformed into independent microservices.

## Troubleshooting

### Compilation Problems

If you encounter errors during compilation, check that:
1. Java JDK is correctly installed and configured
2. Maven is correctly installed and configured
3. Dependencies are correctly defined in the pom.xml file

### Execution Problems

If you encounter errors during execution, check that:
1. The JAR is correctly generated
2. File permissions are correct for writing in the current directory
3. Logs in the app.log file for more details on errors

## Contributing

Contributions are welcome! To contribute:
1. Fork the project
2. Create a branch for your feature (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License. See the LICENSE file for more details.
