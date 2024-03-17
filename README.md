# Welcome
This repository is a practical ASP.NET Core Web API (.NET 8) solution that implements **Onion Architecture**
![](https://code-maze.com/wp-content/uploads/2021/07/onion_architecture.jpeg)

Presentation Layer:
 - CompanyEmployees
 - Presentation

Infrastructure Layer:
 - Repository
 - LoggerService
 
Service Layer:
 - Service
 - Service.Contract
 - Shared

Domain Layer:
 - Contract
 - Entities

## Features

 1. HTTP methods: GET, POST, DELETE, PUT, PATCH, OPTIONS, HEAD
 2. Global error handling
 3. Validation attributes
 4. Asynchronous code
 5. Paging, Filtering, Searching, Sorting
 6. Data shaping
 7. Supporting HATEOAS
 8. Versioning
 9. Output caching
 10. Rate limiting
 11. Authentication & Authorization (JWT + refresh token)
 12. Documenting API with Open API

## How to run

### Prerequisites:
 - .NET 8
 - SQL Server
 ### Create the secret key
 Run the following command in CMD with admin privilege to create secret key to sign/validate tokens.

    setx WEB_API_SECRET "YourTopSecretKeyHasGreater256Bytes113211162023!!!!" /M

Provide your SQL Server connection string in **CompanyEmployees\appsettings.json**

### Run the app
In the solution directory, restore all the dependencies by the .NET CLI command:

    dotnet restore
Navigate to *CompanyEmployees* directory, initialize the sample database:

    dotnet ef database update

Start the app

    dotnet run

Test the APIs by these commands:

    curl https://localhost:5001/api/companies/c9d4c053-49b6-410c-bc78-2d54a9991870
    curl https://localhost:5001/api/companies/c9d4c053-49b6-410c-bc78-2d54a9991870/employees

Open Swagger UI using a web browser to explorer the APIs at: https://localhost:5001/swagger/index.html
