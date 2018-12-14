# EDennis.AspNetCore.Base
*A library with many test/sample projects, used as a basis for creating testable three-tier applications*

## Motivation
ASP.NET Core MVC and Entity Framework Core collectively provide a great framework for development of enterprise applications.  That said, developing integration-testable three-tier applications with these technologies can be a little challenging at times.  It is relatively easy to create a web application on one server that communicates with web API on another server, which in turn communicates with a database via Entity Framework; however, to set up proper, repeatable integration tests -- those that completely reset the database (whether in-memory or real) after each test -- is not straightforward, especially when dealing with three-tier solutions.  Furthermore, to completely reset the database after spot testing via Swagger UI requires some extra set-up work.  Collectively, these challenges motivated development of a library to facilitate testable three-tier application development. 

## Features
The current library has a number of features that assist with application development:
- An action filter that looks for special header values in HTTP requests in order to initialize or reset a test connection or in-memory database.  This action filter supports two kinds of testing approaches:
  1. Testing Transaction Approach, in which an open transaction is rolled back after each test
  2. In-Memory Database Approach, in which the In-Memory database provider is used
- An ApiLauncher class that uses appsettings.json to identify, start, and one or more web applications.  This utility class makes it easier to start and manage a set of micro-services  
- A base repository class that simplifies basic Entity Framework operations but still allows querying via Linq expressions
- A pair of design-time factory classes that allow code-first migrations without default constructors in the DbContext class. (One of these classes, in combination with the EDennis.MigrationsExtensions NuGet library, supports auto-creation of temporal tables.)
- a ResettableValueGenerator class, based upon Arthur Vickers' solution of the same name.  This enhancement resets **to pre-test values** all in-memory IDs backed by either identity specs or sequences
- SQL scripts that identify and reset all identity values and sequences in a SQL Server database
- Extensions to IServiceCollection that use Reflection to simplify the setup of Dependency Injection for
  - Typed HttpClients
  - DbContextPools
  - Repository classes
- Extensions to HttpClient that simplify the setup of 
  - Basic Http operations such as posting, putting, deleting, or getting any object of type T
  - Consecutive Http operations such as posting and then getting an object of type T -- making it easier to use HttpClient for testing.
- Two types of Controllers
  - RepoController, which is designed to work with one or more repository classes backed by Entity Framework
  - ProxyController, which is designed to work with one or more API classes backed by HttpClient instances

## Constraints
- The current library does not support *parallel* integration/unit tests.  The Testing Transaction Approach does not work well with parallel testing because the isolation level is serializable.  The In-Memory Database Approach does not work well with parallel testing because the generated values are using a shared cache.  To ensure that all integration tests are executed sequentially in Xunit, decorate all test classes in a solution with the same attribute: [Collection("Sequential")].


