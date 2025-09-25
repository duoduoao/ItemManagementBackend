# Item Management System

## Overview
The Item Management System is a modern enterprise-grade application built on .NET 9, designed to manage items and categories with secure authentication and robust data handling. It exposes RESTful APIs and is accompanied by an interactive web application.The system follows Clean Architecture principles and includes automated deployment, comprehensive testing, and enterprise-grade security features.


---

## Technology Stack and Configuration

### Core Framework
- **.NET 9.x:** Primary runtime and framework powering the application.
- **ASP.NET Core:** Web API framework for building RESTful services.
- **Entity Framework Core:** ORM with SQL Server provider for relational data management.

### Authentication and Authorization
- **Azure Active Directory (Azure AD):** Provides JWT-based authentication. 

### Supporting Libraries
- **AutoMapper:** Object-to-object mapping configured via `AutoMapperProfiles` and `ViewModelMappingProfile`.
- **FluentValidation:** Validation for DTO inputs to ensure data integrity.
- **Serilog:** Structured logging including file and console outputs for diagnostics.

### API Documentation
- **Swagger / OpenAPI (Swashbuckle.AspNetCore):** Provides interactive API documentation accessible at `/swagger` endpoint.
- Automatically generated API docs for testing and integration.

---

## Getting Started

### Prerequisites
- [.NET 9.x SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- Azure AD Tenant for authentication setup
- Docker Desktop (optional) for Redis caching container

 
