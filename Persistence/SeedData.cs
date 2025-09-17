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
                new User { Id = user1Id, UserName = "Maya Ellington", UserEmail = "maya.ellington@example.com" },
                new User { Id = user2Id, UserName = "Luca Moretti", UserEmail = "luca.moretti@example.net" }
            );

            // Projects
            modelBuilder.Entity<Project>().HasData(
                new Project { Id = project1Id, ProjectName = "Onboarding UX Refresh", ProjectDescription = "Venison cow kielbasa porchetta tri-tip drumstick alcatra capicola. Cow tail alcatra swine frankfurter beef, meatball pork loin short ribs chislic chuck doner shank pig ham." },
                new Project { Id = project2Id, ProjectName = "Process Sync Initiative", ProjectDescription = "Kielbasa drumstick frankfurter corned beef andouille prosciutto hamburger pork chop. Meatball brisket turducken t-bone pancetta." }
            );

            // ProjectUsers (junction table)
            modelBuilder.Entity<ProjectUser>().HasData(
                new ProjectUser { ProjectId = project1Id, UserId = user1Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project1Id, UserId = user2Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project2Id, UserId = user2Id, Role = ProjectRole.Owner }
            );

            // ProjectTasks
            modelBuilder.Entity<ProjectTask>().HasData(
                new ProjectTask { Id = task1Id, ProjectTaskTitle = "- Audit Data Access Permissions\r\n", ProjectTaskDescription = "Ham hock sirloin turducken venison. Picanha pork landjaeger, capicola meatloaf chuck pastrami ham hock. Alcatra ribeye frankfurter drumstick pork. ", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project1Id, AssignedUserId = user2Id },
                new ProjectTask { Id = task2Id, ProjectTaskTitle = "Conduct Vendor Risk Assessment", ProjectTaskDescription = "Kielbasa beef tenderloin pastrami drumstick fatback pork chop. Short ribs ground round brisket, jerky pork chop porchetta biltong pancetta ham hock.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project2Id, AssignedUserId = user1Id }
            );

        }
    }
}
