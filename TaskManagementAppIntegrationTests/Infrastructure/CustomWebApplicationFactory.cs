using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace TaskManagementAppIntegrationTests.Infrastructure;

/// <summary>
/// Custom factory that swaps SQL Server for InMemory database and simplifies authentication for tests.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private static bool _databaseSeeded = false;
    private static readonly object _seedLock = new object();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set environment to Testing BEFORE Program.cs runs
        // This allows Program.cs to conditionally register the database provider
        builder.UseEnvironment("Testing");
        
        // Also configure app configuration to ensure environment is set early
        // Add test configuration values (JWT settings, etc.)
        builder.ConfigureAppConfiguration((context, config) =>
        {
            context.HostingEnvironment.EnvironmentName = "Testing";
            
            // Add in-memory configuration for test environment
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "JWT:Key", "ThisIsA_VeryStrongRandomKey_ChangeIt123!@#_MyOwnSuperSecretKey_1564$Secure!" },
                { "JWT:Issuer", "TaskManagementApp" },
                { "JWT:Audience", "TaskManagementAppAudience" },
                { "JWT:ExpirationInMinutes", "60" }
            });
        });
        
        builder.ConfigureServices(services =>
        {
            // Remove ALL database-related services to prevent provider conflicts
            // This includes DbContext, DbContextOptions, and any provider-specific services
            
            // Remove DbContextOptions<ApplicationDbContext>
            var dbContextOptionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (dbContextOptionsDescriptor != null)
            {
                services.Remove(dbContextOptionsDescriptor);
            }

            // Remove ApplicationDbContext
            var applicationDbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ApplicationDbContext));
            if (applicationDbContextDescriptor != null)
            {
                services.Remove(applicationDbContextDescriptor);
            }

            // Remove all generic DbContextOptions registrations
            var dbContextOptionsDescriptors = services.Where(
                d => d.ServiceType.IsGenericType && 
                d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))
                .ToList();
            foreach (var descriptor in dbContextOptionsDescriptors)
            {
                services.Remove(descriptor);
            }

            // Remove any SQL Server provider services
            var sqlServerServices = services.Where(d =>
                d.ServiceType != null && (
                    d.ServiceType.FullName?.Contains("SqlServer") == true ||
                    d.ServiceType.FullName?.Contains("Microsoft.EntityFrameworkCore.SqlServer") == true ||
                    (d.ImplementationType != null && (
                        d.ImplementationType.FullName?.Contains("SqlServer") == true ||
                        d.ImplementationType.FullName?.Contains("Microsoft.EntityFrameworkCore.SqlServer") == true))
                ))
                .ToList();
            foreach (var descriptor in sqlServerServices)
            {
                services.Remove(descriptor);
            }

            // Remove any database provider services registered by EF Core
            var providerServices = services.Where(d =>
                d.ServiceType != null && (
                    d.ServiceType.FullName?.Contains("IDatabaseProvider") == true ||
                    d.ServiceType.FullName?.Contains("DatabaseProvider") == true ||
                    (d.ImplementationType != null && (
                        d.ImplementationType.FullName?.Contains("DatabaseProvider") == true))
                ))
                .ToList();
            foreach (var descriptor in providerServices)
            {
                services.Remove(descriptor);
            }

            // Use EF Core InMemory database (simpler, no provider conflicts)
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Register test authentication scheme
            // (JWT Bearer is not registered in Testing environment, so no need to remove it)
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", _ => { });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder("Test")
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // Seed database after services are configured
            var sp = services.BuildServiceProvider();
            lock (_seedLock)
            {
                if (!_databaseSeeded)
                {
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.EnsureCreated();
                    
                    // Ensure test users exist
                    var hasher = new PasswordHasher<User>();
                    
                    if (!db.Users.Any(u => u.UserEmail == "test@example.com"))
                    {
                        var user1 = new User
                        {
                            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            UserName = "test-user",
                            UserEmail = "test@example.com",
                        };
                        user1.PasswordHash = hasher.HashPassword(user1, "Password123!");
                        db.Users.Add(user1);
                    }

                    if (!db.Users.Any(u => u.UserEmail == "test2@example.com"))
                    {
                        var user2 = new User
                        {
                            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            UserName = "test-user-2",
                            UserEmail = "test2@example.com",
                        };
                        user2.PasswordHash = hasher.HashPassword(user2, "Password123!");
                        db.Users.Add(user2);
                    }
                    
                    db.SaveChanges();
                    _databaseSeeded = true;
                }
            }
        });
    }

}

