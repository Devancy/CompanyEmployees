# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CompanyEmployees/CompanyEmployees.csproj", "CompanyEmployees/"]
COPY ["CompanyEmployees.Presentation/CompanyEmployees.Presentation.csproj", "CompanyEmployees.Presentation/"]
COPY ["Service.Contracts/Service.Contracts.csproj", "Service.Contracts/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["LoggerService/LoggerService.csproj", "LoggerService/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Service/Service.csproj", "Service/"]
RUN dotnet restore "./CompanyEmployees/CompanyEmployees.csproj"
COPY . .
WORKDIR "/src/CompanyEmployees"
RUN dotnet build "./CompanyEmployees.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CompanyEmployees.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompanyEmployees.dll"]