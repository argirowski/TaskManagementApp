using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Repositories;
using FluentAssertions;
using Persistence;

namespace TaskManagementAppRepositoryTests.Repositories
{
    public class UserRepositoryTests
    {
        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddUserAsync_SavesUser()
        {
            using var context = CreateDbContext();
            var repo = new UserRepository(context);
            var user = new User { Id = Guid.NewGuid(), UserName = "TestUser", UserEmail = "test@mail.com", PasswordHash = "hash" };

            await repo.AddUserAsync(user);

            context.Users.Count().Should().Be(1);
            context.Users.First().UserName.Should().Be("TestUser");
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsUser()
        {
            using var context = CreateDbContext();
            var user = new User { Id = Guid.NewGuid(), UserName = "TestUser", UserEmail = "test@mail.com", PasswordHash = "hash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repo = new UserRepository(context);

            var result = await repo.GetByEmailAsync("test@mail.com");
            result.Should().NotBeNull();
            result.UserName.Should().Be("TestUser");
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser()
        {
            using var context = CreateDbContext();
            var user = new User { Id = Guid.NewGuid(), UserName = "TestUser", UserEmail = "test@mail.com", PasswordHash = "hash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repo = new UserRepository(context);

            user.UserName = "Updated";
            await repo.UpdateUserAsync(user);
            context.Users.First().UserName.Should().Be("Updated");
        }

        [Fact]
        public async Task GetUserByRefreshTokenAsync_ReturnsUserIfValid()
        {
            using var context = CreateDbContext();
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                UserName = "TestUser",
                UserEmail = "test@mail.com",
                PasswordHash = "hash",
                RefreshToken = "token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(10)
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repo = new UserRepository(context);

            var result = await repo.GetUserByRefreshTokenAsync("token", userId);
            result.Should().NotBeNull();
            result.UserName.Should().Be("TestUser");
        }

        [Fact]
        public async Task GetUserByRefreshTokenAsync_ReturnsNullIfExpired()
        {
            using var context = CreateDbContext();
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                UserName = "TestUser",
                UserEmail = "test@mail.com",
                PasswordHash = "hash",
                RefreshToken = "token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(-10)
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repo = new UserRepository(context);

            var result = await repo.GetUserByRefreshTokenAsync("token", userId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser()
        {
            using var context = CreateDbContext();
            var user = new User { Id = Guid.NewGuid(), UserName = "TestUser", UserEmail = "test@mail.com", PasswordHash = "hash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            var repo = new UserRepository(context);

            var result = await repo.GetByIdAsync(user.Id);
            result.Should().NotBeNull();
            result.UserName.Should().Be("TestUser");
        }
    }
}
