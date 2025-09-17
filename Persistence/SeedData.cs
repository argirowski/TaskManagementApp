using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {    // Fixed IDs to keep relationships consistent
            var user1Id = Guid.Parse("be2f07b8-c92d-4138-b945-8fefecbc3cbe");
            var user2Id = Guid.Parse("6c8cd30a-cca3-4537-a8dd-7b6ac903bf29");
            var project1Id = Guid.Parse("15d4a262-aac1-418b-b32e-0bffd37966d5");
            var project2Id = Guid.Parse("37eac064-228b-45d8-bc19-9d09b94067b9");
            var task1Id = Guid.Parse("e79e1167-8ba5-4c8e-afe9-9b3dcf69df2a");
            var task2Id = Guid.Parse("724981b4-fe11-4247-8a51-2fdb56d16a50");

            // Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = user1Id, UserName = "alice", UserEmail = "alice@test.com" },
                new User { Id = user2Id, UserName = "bob", UserEmail = "bob@test.com" }
            );

            // Projects
            modelBuilder.Entity<Project>().HasData(
                new Project { Id = project1Id, ProjectName = "Website Redesign", ProjectDescription = "Redesign the corporate website" },
                new Project { Id = project2Id, ProjectName = "API Migration", ProjectDescription = "Migrate the legacy API to a new framework" }
            );

            // ProjectUsers (junction table)
            modelBuilder.Entity<ProjectUser>().HasData(
                new ProjectUser { ProjectId = project1Id, UserId = user1Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project1Id, UserId = user2Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project2Id, UserId = user2Id, Role = ProjectRole.Owner }
            );

            // Tasks
            modelBuilder.Entity<ProjectTask>().HasData(
                new ProjectTask { Id = task1Id, ProjectTaskTitle = "Design homepage", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project1Id, AssignedUserId = user2Id },
                new ProjectTask { Id = task2Id, ProjectTaskTitle = "Migrate database", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project2Id, AssignedUserId = user1Id }
            );

        }
    }
}
