# .NET Core Console Application

This project is a console application developed in .NET Core that demonstrates the use of reusable modules and a modular architecture. The application allows performing simple operations such as date formatting, string manipulation, arithmetic calculations, and file writing.

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Building](#building)
- [Running](#running)
- [Testing](#testing)
- [Project Structure](#project-structure)
- [Module Details](#module-details)
- [Test Details](#test-details)
- [Development Best Practices](#development-best-practices)
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

The application is divided into several components:
- **Main Program**: Coordinates the use of different modules
- **Utility Classes**: Provide basic functionalities (DateUtils, StringUtils)
- **Functional Modules**: Implement business functionalities (Calculator, FileHandler)
- **Data Models**: Represent data structures used (DateModel)

## Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2022 or Visual Studio Code with C# extension

## Installation

### Installing .NET SDK

#### Windows
1. Download and install .NET SDK from [Microsoft's official website](https://dotnet.microsoft.com/download)
2. Verify installation by running `dotnet --version` in a command prompt

#### Linux (Ubuntu/Debian)
```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

# Install .NET SDK
sudo apt-get update
sudo apt-get install -y apt-transport-https
sudo apt-get install -y dotnet-sdk-6.0
```

#### macOS
```bash
# Using Homebrew
brew install --cask dotnet-sdk
```

## Building

To build the project:

```bash
# Navigate to the project directory
cd CobolApp.Net

# Build the solution
dotnet build
```

## Running

To run the application:

```bash
# Navigate to the project directory
cd CobolApp.Net/src/CobolApp.Net

# Run the application
dotnet run
```

## Testing

To run the tests:

```bash
# Navigate to the project directory
cd CobolApp.Net

# Run all tests
dotnet test

# Run specific test project
dotnet test src/CobolApp.Net.Tests
```

## Project Structure

```
CobolApp.Net/
├── CobolApp.Net.sln                      # Solution file
├── src/
│   ├── CobolApp.Net/                     # Main application project
│   │   ├── CobolApp.Net.csproj           # Project file
│   │   ├── Program.cs                    # Main program
│   │   ├── Models/
│   │   │   └── DateModel.cs              # Date model
│   │   ├── Modules/
│   │   │   ├── Calculator.cs             # Calculator module
│   │   │   └── FileHandler.cs            # File handler module
│   │   └── Utils/
│   │       ├── DateUtils.cs              # Date utilities
│   │       └── StringUtils.cs            # String utilities
│   └── CobolApp.Net.Tests/               # Test project
│       ├── CobolApp.Net.Tests.csproj     # Test project file
│       ├── Modules/
│       │   ├── CalculatorTests.cs        # Calculator tests
│       │   └── FileHandlerTests.cs       # File handler tests
│       └── Utils/
│           ├── DateUtilsTests.cs         # Date utils tests
│           └── StringUtilsTests.cs       # String utils tests
└── docs/
    ├── images/                           # Generated diagrams
    └── diagrams/                         # Diagram source files
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

### DateUtils Tests

These tests verify that the DateUtils module correctly formats a date. They define a test date (15/05/2023) and verify that the formatting result matches the expected value.

### StringUtils Tests

These tests verify that the StringUtils module correctly generates a greeting message. They define a username ("Jean") and verify that the generated message matches the expected value ("Bonjour, Jean !").

### Calculator Tests

These tests verify that the Calculator module correctly adds two numbers. They define two numbers (123.45 and 67.89) and verify that their calculated sum matches the expected value (191.34).

### FileHandler Tests

These tests verify that the FileHandler module correctly writes content to a file. They define a filename and content, call the module to write the file, then verify that the file was created and its content matches the expected value.

## Development Best Practices

This project follows several .NET development best practices:

1. **Modularity**: The code is organized into reusable modules with well-defined responsibilities.
2. **Unit Testing**: Each module has unit tests to verify its proper functioning.
3. **Error Handling**: Proper exception handling and status codes to indicate success or failure.
4. **Documentation**: The code is well-documented with XML comments.
5. **Consistent Naming**: Variables and modules follow .NET naming conventions.
6. **Separation of Concerns**: Functionalities are separated into distinct modules according to their responsibility.
7. **Immutability**: Using immutable patterns where appropriate.
8. **Resource Management**: Proper resource disposal with using statements.

## Integration with Other Systems

This application can be integrated with other systems in several ways:

1. **REST API**: The application can be extended to expose its functionalities via ASP.NET Core Web API.
2. **Databases**: The application can be connected to databases via Entity Framework Core.
3. **Messaging**: The application can be integrated with messaging systems like RabbitMQ or Azure Service Bus.
4. **Web Services**: The application can consume or expose SOAP or REST web services.
5. **Microservices**: The modules can be transformed into independent microservices.

## Troubleshooting

### Build Problems

If you encounter errors during build:
1. Ensure .NET SDK is correctly installed
2. Restore NuGet packages with `dotnet restore`
3. Check for syntax errors in the code

### Runtime Problems

If you encounter errors during execution:
1. Check file permissions for writing output files
2. Verify input formats for numbers and dates
3. Check the application logs for detailed error information

## Contributing

Contributions are welcome! To contribute:
1. Fork the project
2. Create a branch for your feature (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License. See the LICENSE file for more details.
