# LunaEdge.TestAssignment

![example workflow](https://github.com/Aristocrab/LunaEdge.TestAssignment/actions/workflows/dotnet.yml/badge.svg)

## Used technologies
- .NET 9
- [Aspire](https://github.com/dotnet/aspire), Docker, Docker Compose
- ASP.NET Core, EF Core, PostgreSQL, Specification Pattern
- xunit, NSubstitute, FluentAssertions, Bogus
- Swagger, Serilog, FluentValidation, Scrutor, [Throw](https://github.com/amantinband/throw)

## How to run

### Aspire
Run project `LunaEdge.TestAssignment.AppHost` (requires Docker to be running)

### Docker
Run `docker-compose up --build` in the root directory

## Architecture
- aspire
  - **LunaEdge.TestAssignment.AppHost** - Aspire project, contains all the configurations and services
  - **LunaEdge.TestAssignment.ServiceDefaults** - Aspire shared project
- api
  - **LunaEdge.TestAssignment.Domain** - Contains all the domain models, enums, and exceptions
  - **LunaEdge.TestAssignment.Application** - Contains all the application services and db context
  - **LunaEdge.TestAssignment.WebApi** - Contains all the controllers and DI configurations

## API
- `POST` `/users/register`: To register a new user.
```json
{
  "email": "string@gmail.com",
  "password": "q123456t!",
  "username": "username123"
}
  ```
- `POST` `/users/login`: To authenticate a user and return a JWT.
```json
{
  "usernameOrEmail": "username123",
  "password": "q123456t!"
}
```
- `POST` `/tasks`: To create a new task (authenticated).
```json
{
  "title": "Title",
  "description": "Lorem ipsum dolor sit amet",
  "dueDate": "2024-12-28T23:28:45.540Z",
  "status": 1,
  "priority": 2
}
```
- `GET` `/tasks`: To retrieve a list of tasks for the authenticated user, with optional filters.
```
tasks?page=2&pageSize=10&status=1&priority=1&sortBy=priority&isAscending=true
```
- `GET` `/tasks/{id}`: To retrieve the details of a specific task by its ID (authenticated).
- `PUT` `/tasks/{id}`: To update an existing task (authenticated).
- `DELETE` `/tasks/{id}`: To delete a specific task by its ID (authenticated).
