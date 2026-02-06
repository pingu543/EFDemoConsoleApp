using EFDemoConsoleApp.Data;
using EFDemoConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("EF Core Code-First Demo: User Table");

// READ 1.
// We are using code-first approach to manage the shape of the database.
// Required NuGet packages:
// - Microsoft.EntityFrameworkCore.SqlServer — SQL Server database provider
// - Microsoft.EntityFrameworkCore.Design — enables migrations
// - Microsoft.EntityFrameworkCore.Tools — Package Manager Console commands
// - Microsoft.Extensions.Configuration.Json — appsettings.json support
// - Microsoft.Extensions.DependencyInjection — dependency injection framework
// - Microsoft.Extensions.Hosting — modern .NET host with DI and configuration
// DependencyInjection and Hosting are included by default in ASP.NET Core, but in console app we need to add them manually.

// READ 2.
// Build the application host with configuration and dependency injection.
// This is the modern .NET way and matches ASP.NET Core patterns.
// The host automatically loads appsettings.json and sets up dependency injection.
var builder = Host.CreateApplicationBuilder(args);

// READ 3.
// Register the DbContext with dependency injection.
// The connection string is automatically loaded from appsettings.json.
// This approach separates configuration from code and makes testing easier.
// In console app, the appsettings.json's property needs to be changed to copy if newer manually.
// appsettings.json is also suppressing the info console output of the database.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var host = builder.Build();

// READ 4.
// Create a service scope to resolve dependencies.
// This is required because DbContext has a scoped lifetime.
// Proceed to READ 5 in \Data\AppDbContext.cs
using var scope = host.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

// READ 10.
// Apply any pending migrations to the database.
// Basically, MigrateAsync() will do an Update-Database on application startup.
// If the database doesn't exist, it will be created.
// For this to work, you must first run:
// Add-Migration InitialCreate
// The Factory in AppDbContextFactory.cs enables design-time support for migrations.
// The Factory is not needed in ASP.NET Core because the runtime can create the DbContext from the registered services,
// but in console app we need it to run migrations from Package Manager Console.
// Proceed to READ 11 in \Data\AppDbContextFactory.cs.
await context.Database.MigrateAsync();

// READ 12.
// Seed sample data if the database is empty.
if (!context.Users.Any())
{
    var users = new[]
    {
        new User { Email = "person1@example.com" },
        new User { Email = "person2@example.com" },
        new User { Email = "person3@example.com" }
    };
    
    context.Users.AddRange(users);
    await context.SaveChangesAsync();
    Console.WriteLine($"Created {users.Length} users");
}

// READ 13.
// Query and display all users from the database.
var allUsers = await context.Users.ToListAsync();

Console.WriteLine("\nUsers in database:");
foreach (var user in allUsers)
{
    Console.WriteLine($"ID: {user.Id}, Email: {user.Email}");
}
