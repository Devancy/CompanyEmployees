# CompanyEmployees API

## Enterprise-Grade ASP.NET Core Web API (.NET 8) with Onion Architecture

A robust RESTful API solution demonstrating industry best practices and advanced architectural patterns using .NET 8. This project implements a comprehensive employee management system with enterprise-level features and clean architecture principles.

![Onion Architecture](https://code-maze.com/wp-content/uploads/2021/07/onion_architecture.jpeg)

## Architecture Overview

This solution strictly adheres to the **Onion Architecture** pattern, providing clear separation of concerns:

### Domain Layer (Core)
- **Contracts**: Interface definitions enforcing business rules
- **Entities**: Domain models and business objects
- **Shared**: Common DTOs and shared resources

### Service Layer
- **Service.Contracts**: Service interfaces
- **Service**: Business logic implementation

### Infrastructure Layer
- **Repository**: Data access implementation and database context
- **LoggerService**: Logging mechanism

### Presentation Layer
- **CompanyEmployees**: Main application entry point and configuration
- **CompanyEmployees.Presentation**: API controllers and presentation logic

## Technical Highlights

### Advanced API Features
- Complete RESTful API implementation (GET, POST, PUT, PATCH, DELETE, OPTIONS, HEAD)
- Authentication & Authorization with JWT and refresh token mechanism
- API versioning with support for multiple versioning strategies
- HATEOAS (Hypermedia as the Engine of Application State) implementation
- Comprehensive API documentation with OpenAPI/Swagger

### Performance & Scalability
- Output caching for optimized response times
- Rate limiting to protect against abuse
- Asynchronous code execution throughout the application
- Advanced data retrieval options (Paging, Filtering, Searching, Sorting)

### Data Handling
- Data shaping capabilities for efficient data transfer
- Entity Framework Core with code-first approach
- Repository pattern for data access abstraction

### Security & Best Practices
- Global exception handling with custom error responses
- Input validation using attributes and model validation
- Docker containerization support

## Setup & Deployment

### Prerequisites
- .NET 8 SDK
- SQL Server (or SQL Server Express)

### Security Configuration

#### Windows Environment
```bash
# Create JWT secret key (run as Administrator)
setx WEB_API_SECRET "YourTopSecretKeyHasGreater256Bytes113211162023!!!!" /M
```

#### Docker/Container Environment
Secret management via environment variables in docker-compose.yml:
```yaml
services:
  companyemployees:
    environment:
      - WEB_API_SECRET=YourTopSecretKeyHasGreater256Bytes113211162023!!!!
```

### Database Setup
1. Configure your SQL Server connection string in `CompanyEmployees\appsettings.json`
2. Initialize the database:
```bash
dotnet ef database update
```

### Running the Application
```bash
# Restore dependencies
dotnet restore

# Start the API
dotnet run --project CompanyEmployees
```

### API Documentation
Explore the complete API using Swagger UI: https://localhost:5001/swagger/index.html
or if you're running in docker environment: https://localhost:8081/swagger/index.html
