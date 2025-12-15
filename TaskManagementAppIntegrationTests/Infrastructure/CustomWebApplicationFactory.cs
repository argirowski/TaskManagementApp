using System.Linq;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace TaskManagementAppIntegrationTests.Infrastructure;

/// <summary>
/// Custom factory that swaps SQL Server for InMemory database and simplifies authentication for tests.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private static bool _databaseInitialized = false;
    private static readonly object _lock = new object();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration (remove by predicate to catch all related services)
            var descriptors = services.Where(d => 
                d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                d.ServiceType == typeof(ApplicationDbContext) ||
                (d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)))
                .ToList();
            
            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            // Use EF Core InMemory database (simpler, no provider conflicts)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));

            // Replace authentication with test scheme
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

            // Seed database after the service provider is built
            var sp = services.BuildServiceProvider();
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    if (!db.Users.Any())
                    {
                        var user = new User
                        {
                            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            UserName = "test-user",
                            UserEmail = "test@example.com",
                        };
                        var hasher = new PasswordHasher<User>();
                        user.PasswordHash = hasher.HashPassword(user, "Password123!");
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    _databaseInitialized = true;
                }
            }
        });
    }

}

