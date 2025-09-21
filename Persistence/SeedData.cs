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
            var user3Id = Guid.Parse("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f");
            var user4Id = Guid.Parse("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a");
            var user5Id = Guid.Parse("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b");
            var user6Id = Guid.Parse("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c");
            var user7Id = Guid.Parse("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d");
            var user8Id = Guid.Parse("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e");
            var user9Id = Guid.Parse("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f");
            var user10Id = Guid.Parse("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a");

            var project1Id = Guid.Parse("15d4a262-aac1-418b-b32e-0bffd37966d5");
            var project2Id = Guid.Parse("37eac064-228b-45d8-bc19-9d09b94067b9");
            var project3Id = Guid.Parse("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d");
            var project4Id = Guid.Parse("b2c3d4e5-f6a7-5b8c-9d0e-1f2a3b4c5d6e");
            var project5Id = Guid.Parse("c3d4e5f6-a7b8-6c9d-0e1f-2a3b4c5d6e7f");
            var project6Id = Guid.Parse("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a");
            var project7Id = Guid.Parse("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b");
            var project8Id = Guid.Parse("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c");
            var project9Id = Guid.Parse("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d");
            var project10Id = Guid.Parse("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e");
            var project11Id = Guid.Parse("c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f");
            var project12Id = Guid.Parse("d0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a");
            var project13Id = Guid.Parse("e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b");
            var project14Id = Guid.Parse("f2a3b4c5-d6e7-5f8a-9b0c-1d2e3f4a5b6c");
            var project15Id = Guid.Parse("a3b4c5d6-e7f8-6a9b-0c1d-2e3f4a5b6c7d");
            var project16Id = Guid.Parse("b4c5d6e7-f8a9-7b0c-1d2e-3f4a5b6c7d8e");
            var project17Id = Guid.Parse("c5d6e7f8-a9b0-8c1d-2e3f-4a5b6c7d8e9f");
            var project18Id = Guid.Parse("d6e7f8a9-b0c1-9d2e-3f4a-5b6c7d8e9f0a");
            var project19Id = Guid.Parse("e7f8a9b0-c1d2-0e3f-4a5b-6c7d8e9f0a1b");
            var project20Id = Guid.Parse("f8a9b0c1-d2e3-1f4a-5b6c-7d8e9f0a1b2c");

            var task1Id = Guid.Parse("e79e1167-8ba5-4c8e-afe9-9b3dcf69df2a");
            var task2Id = Guid.Parse("724981b4-fe11-4247-8a51-2fdb56d16a50");
            var task3Id = Guid.Parse("a1e2f3d4-1a2b-4c3d-9e1f-1a2b3c4d5e6f");
            var task4Id = Guid.Parse("b2f3a4e5-2b3c-5d4e-8f2a-2b3c4d5e6f7a");
            var task5Id = Guid.Parse("c3e4b5f6-3c4d-6e5f-7a3b-3c4d5e6f7a8b");
            var task6Id = Guid.Parse("d4f5c6a7-4d5e-7f6a-9b4c-4d5e6f7a8b9c");
            var task7Id = Guid.Parse("e5a6d7b8-5e6f-8a7b-1c5d-5e6f7a8b9c1d");
            var task8Id = Guid.Parse("f6b7e8c9-6f7a-9b8c-2d6e-6f7a8b9c1d2e");
            var task9Id = Guid.Parse("a7c8f9d0-7a8b-1c9d-3e7f-7a8b9c1d2e3f");
            var task10Id = Guid.Parse("b8d9a1e2-8b9c-2d1e-4f8a-8b9c1d2e3f4a");
            var task11Id = Guid.Parse("c9e1b2f3-9c1d-3e2f-5a9b-9c1d2e3f4a5b");
            var task12Id = Guid.Parse("d0f2c3a4-0c1d-4e3f-6b0c-0c1d2e3f4a5b");
            var task13Id = Guid.Parse("e1a3d4b5-1d2e-5f4a-7c1d-1d2e3f4a5b6c");
            var task14Id = Guid.Parse("f2b4e5c6-2e3f-6a5b-8d2e-2e3f4a5b6c7d");
            var task15Id = Guid.Parse("a3c5f6d7-3f4a-7b6c-9e3f-3f4a5b6c7d8e");
            var task16Id = Guid.Parse("b4d6a7e8-4a5b-8c7d-1f4a-4a5b6c7d8e9f");
            var task17Id = Guid.Parse("c5e7b8f9-5b6c-9d8e-2a5b-5b6c7d8e9f0a");
            var task18Id = Guid.Parse("d6f8c9a1-6c7d-0e9f-3b6c-6c7d8e9f0a1b");
            var task19Id = Guid.Parse("e7a9d0b2-7d8e-1f0a-4c7d-7d8e9f0a1b2c");
            var task20Id = Guid.Parse("f8b0e1c3-8e9f-2a1b-5d8e-8e9f0a1b2c3d");
            var task21Id = Guid.Parse("a2b3c4d5-2a3b-4c5d-6e7f-8a9b0c1d2e3f");
            var task22Id = Guid.Parse("b3c4d5e6-3b4c-5d6e-7f8a-9b0c1d2e3f4a");
            var task23Id = Guid.Parse("c4d5e6f7-4c5d-6e7f-8a9b-0c1d2e3f4a5b");
            var task24Id = Guid.Parse("d5e6f7a8-5d6e-7f8a-9b0c-1d2e3f4a5b6c");
            var task25Id = Guid.Parse("e6f7a8b9-6e7f-8a9b-0c1d-2e3f4a5b6c7d");
            var task26Id = Guid.Parse("f7a8b9c0-7f8a-9b0c-1d2e-3f4a5b6c7d8e");
            var task27Id = Guid.Parse("a8b9c0d1-8a9b-0c1d-2e3f-4a5b6c7d8e9f");
            var task28Id = Guid.Parse("b9c0d1e2-9b0c-1d2e-3f4a-5b6c7d8e9f0a");
            var task29Id = Guid.Parse("c0d1e2f3-0c1d-2e3f-4a5b-6c7d8e9f0a1b");
            var task30Id = Guid.Parse("d1e2f3a4-1d2e-3f4a-5b6c-7d8e9f0a1b2c");
            var task31Id = Guid.Parse("e2f3a4b5-2e3f-4a5b-6c7d-8e9f0a1b2c3d");
            var task32Id = Guid.Parse("f3a4b5c6-3f4a-5b6c-7d8e-9f0a1b2c3d4e");

            // Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = user1Id, UserName = "Maya Ellington", UserEmail = "maya.ellington@example.com" },
                new User { Id = user2Id, UserName = "Luca Moretti", UserEmail = "luca.moretti@example.net" },
                new User { Id = user3Id, UserName = "Sofia Ivanova", UserEmail = "sofia.ivanova@example.org" },
                new User { Id = user4Id, UserName = "James O'Connor", UserEmail = "james.oconnor@example.com" },
                new User { Id = user5Id, UserName = "Aisha Patel", UserEmail = "aisha.patel@example.net" },
                new User { Id = user6Id, UserName = "Carlos Mendes", UserEmail = "carlos.mendes@example.org" },
                new User { Id = user7Id, UserName = "Emma Johansson", UserEmail = "emma.johansson@example.com" },
                new User { Id = user8Id, UserName = "Chen Wei", UserEmail = "chen.wei@example.net" },
                new User { Id = user9Id, UserName = "Fatima Al-Farsi", UserEmail = "fatima.alfarsi@example.org" },
                new User { Id = user10Id, UserName = "David Kim", UserEmail = "david.kim@example.com" }
            );

            // Projects
            modelBuilder.Entity<Project>().HasData(
                new Project { Id = project1Id, ProjectName = "Onboarding UX Refresh", ProjectDescription = "Venison cow kielbasa porchetta tri-tip drumstick alcatra capicola. Cow tail alcatra swine frankfurter beef, meatball pork loin short ribs chislic chuck doner shank pig ham." },
                new Project { Id = project2Id, ProjectName = "Process Sync Initiative", ProjectDescription = "Kielbasa drumstick frankfurter corned beef andouille prosciutto hamburger pork chop. Meatball brisket turducken t-bone pancetta." },
                new Project { Id = project3Id, ProjectName = "Mobile App Redesign", ProjectDescription = "Revamp the mobile app UI/UX for better engagement and retention." },
                new Project { Id = project4Id, ProjectName = "Cloud Migration", ProjectDescription = "Migrate legacy systems to cloud infrastructure for scalability." },
                new Project { Id = project5Id, ProjectName = "API Gateway Implementation", ProjectDescription = "Centralize API management and security with a gateway solution." },
                new Project { Id = project6Id, ProjectName = "Data Analytics Platform", ProjectDescription = "Build a platform for advanced business intelligence and analytics." },
                new Project { Id = project7Id, ProjectName = "Customer Portal Launch", ProjectDescription = "Develop a self-service portal for customers to manage accounts." },
                new Project { Id = project8Id, ProjectName = "DevOps Automation", ProjectDescription = "Automate CI/CD pipelines and infrastructure provisioning." },
                new Project { Id = project9Id, ProjectName = "Security Audit", ProjectDescription = "Conduct a comprehensive security audit of all systems." },
                new Project { Id = project10Id, ProjectName = "HR System Upgrade", ProjectDescription = "Upgrade HR software for improved payroll and benefits management." },
                new Project { Id = project11Id, ProjectName = "E-commerce Expansion", ProjectDescription = "Expand e-commerce platform to new markets and regions." },
                new Project { Id = project12Id, ProjectName = "AI Chatbot Integration", ProjectDescription = "Integrate AI-powered chatbot for customer support." },
                new Project { Id = project13Id, ProjectName = "Marketing Automation", ProjectDescription = "Implement marketing automation tools for campaigns." },
                new Project { Id = project14Id, ProjectName = "Inventory Optimization", ProjectDescription = "Optimize inventory management using predictive analytics." },
                new Project { Id = project15Id, ProjectName = "Remote Work Enablement", ProjectDescription = "Enable secure remote work for all employees." },
                new Project { Id = project16Id, ProjectName = "Compliance Tracker", ProjectDescription = "Develop a tool to track regulatory compliance tasks." },
                new Project { Id = project17Id, ProjectName = "Performance Review System", ProjectDescription = "Create a system for employee performance reviews and feedback." },
                new Project { Id = project18Id, ProjectName = "Supplier Management", ProjectDescription = "Streamline supplier onboarding and management processes." },
                new Project { Id = project19Id, ProjectName = "Knowledge Base", ProjectDescription = "Build an internal knowledge base for documentation and FAQs." },
                new Project { Id = project20Id, ProjectName = "Incident Response Planning", ProjectDescription = "Plan and document incident response procedures for IT." }
            );

            // ProjectUsers (junction table)
            modelBuilder.Entity<ProjectUser>().HasData(
                // Each user is assigned to at least one project
                new ProjectUser { ProjectId = project1Id, UserId = user1Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project2Id, UserId = user2Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project3Id, UserId = user3Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project4Id, UserId = user4Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project5Id, UserId = user5Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project6Id, UserId = user6Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project7Id, UserId = user7Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project8Id, UserId = user8Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project9Id, UserId = user9Id, Role = ProjectRole.Owner },
                new ProjectUser { ProjectId = project10Id, UserId = user10Id, Role = ProjectRole.Owner },
                // Add some members to projects for variety
                new ProjectUser { ProjectId = project1Id, UserId = user2Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project2Id, UserId = user3Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project3Id, UserId = user4Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project4Id, UserId = user5Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project5Id, UserId = user6Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project6Id, UserId = user7Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project7Id, UserId = user8Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project8Id, UserId = user9Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project9Id, UserId = user10Id, Role = ProjectRole.Member },
                new ProjectUser { ProjectId = project10Id, UserId = user1Id, Role = ProjectRole.Member }
            );

            // ProjectTasks
            modelBuilder.Entity<ProjectTask>().HasData(
                new ProjectTask { Id = task1Id, ProjectTaskTitle = "- Audit Data Access Permissions\r\n", ProjectTaskDescription = "Ham hock sirloin turducken venison. Picanha pork landjaeger, capicola meatloaf chuck pastrami ham hock. Alcatra ribeye frankfurter drumstick pork. ", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project1Id, AssignedUserId = user2Id },
                new ProjectTask { Id = task2Id, ProjectTaskTitle = "Conduct Vendor Risk Assessment", ProjectTaskDescription = "Kielbasa beef tenderloin pastrami drumstick fatback pork chop. Short ribs ground round brisket, jerky pork chop porchetta biltong pancetta ham hock.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project2Id, AssignedUserId = user1Id },
                new ProjectTask { Id = task3Id, ProjectTaskTitle = "Design Mobile App Wireframes", ProjectTaskDescription = "Create wireframes for the new mobile app redesign.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project3Id, AssignedUserId = user3Id },
                new ProjectTask { Id = task4Id, ProjectTaskTitle = "Cloud Migration Planning", ProjectTaskDescription = "Draft migration plan for legacy systems.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project4Id, AssignedUserId = user4Id },
                new ProjectTask { Id = task5Id, ProjectTaskTitle = "API Gateway Setup", ProjectTaskDescription = "Set up API gateway and configure endpoints.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project5Id, AssignedUserId = user5Id },
                new ProjectTask { Id = task6Id, ProjectTaskTitle = "Analytics Dashboard Development", ProjectTaskDescription = "Develop dashboard for analytics platform.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project6Id, AssignedUserId = user6Id },
                new ProjectTask { Id = task7Id, ProjectTaskTitle = "Customer Portal Authentication", ProjectTaskDescription = "Implement authentication for customer portal.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project7Id, AssignedUserId = user7Id },
                new ProjectTask { Id = task8Id, ProjectTaskTitle = "DevOps Pipeline Automation", ProjectTaskDescription = "Automate CI/CD pipeline for deployments.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project8Id, AssignedUserId = user8Id },
                new ProjectTask { Id = task9Id, ProjectTaskTitle = "Security Audit Checklist", ProjectTaskDescription = "Prepare checklist for security audit.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project9Id, AssignedUserId = user9Id },
                new ProjectTask { Id = task10Id, ProjectTaskTitle = "HR System Data Migration", ProjectTaskDescription = "Migrate data to new HR system.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project10Id, AssignedUserId = user10Id },
                new ProjectTask { Id = task11Id, ProjectTaskTitle = "E-commerce Market Analysis", ProjectTaskDescription = "Analyze new market opportunities for e-commerce.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project11Id, AssignedUserId = user1Id },
                new ProjectTask { Id = task12Id, ProjectTaskTitle = "Chatbot Training Data", ProjectTaskDescription = "Prepare training data for AI chatbot.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project12Id, AssignedUserId = user2Id },
                new ProjectTask { Id = task13Id, ProjectTaskTitle = "Marketing Automation Setup", ProjectTaskDescription = "Set up marketing automation tools.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project13Id, AssignedUserId = user3Id },
                new ProjectTask { Id = task14Id, ProjectTaskTitle = "Inventory Data Import", ProjectTaskDescription = "Import inventory data for optimization.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project14Id, AssignedUserId = user4Id },
                new ProjectTask { Id = task15Id, ProjectTaskTitle = "Remote Work Policy Draft", ProjectTaskDescription = "Draft remote work policy for employees.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project15Id, AssignedUserId = user5Id },
                new ProjectTask { Id = task16Id, ProjectTaskTitle = "Compliance Tracker UI", ProjectTaskDescription = "Design UI for compliance tracker tool.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project16Id, AssignedUserId = user6Id },
                new ProjectTask { Id = task17Id, ProjectTaskTitle = "Performance Review Template", ProjectTaskDescription = "Create template for performance reviews.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project17Id, AssignedUserId = user7Id },
                new ProjectTask { Id = task18Id, ProjectTaskTitle = "Supplier Onboarding Docs", ProjectTaskDescription = "Prepare onboarding documentation for suppliers.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project18Id, AssignedUserId = user8Id },
                new ProjectTask { Id = task19Id, ProjectTaskTitle = "Knowledge Base Structure", ProjectTaskDescription = "Define structure for internal knowledge base.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project19Id, AssignedUserId = user9Id },
                new ProjectTask { Id = task20Id, ProjectTaskTitle = "Incident Response Playbook", ProjectTaskDescription = "Draft playbook for incident response.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project20Id, AssignedUserId = user10Id },
                new ProjectTask { Id = task21Id, ProjectTaskTitle = "Mobile App Testing", ProjectTaskDescription = "Test mobile app features and report bugs.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project3Id, AssignedUserId = user4Id },
                new ProjectTask { Id = task22Id, ProjectTaskTitle = "Cloud Migration Execution", ProjectTaskDescription = "Execute migration of selected services.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project4Id, AssignedUserId = user5Id },
                new ProjectTask { Id = task23Id, ProjectTaskTitle = "API Gateway Monitoring", ProjectTaskDescription = "Monitor API gateway traffic and errors.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project5Id, AssignedUserId = user6Id },
                new ProjectTask { Id = task24Id, ProjectTaskTitle = "Analytics Data Validation", ProjectTaskDescription = "Validate analytics data sources.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project6Id, AssignedUserId = user7Id },
                new ProjectTask { Id = task25Id, ProjectTaskTitle = "Customer Portal Feedback", ProjectTaskDescription = "Collect feedback from portal users.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project7Id, AssignedUserId = user8Id },
                new ProjectTask { Id = task26Id, ProjectTaskTitle = "DevOps Tooling Review", ProjectTaskDescription = "Review DevOps tools for automation.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project8Id, AssignedUserId = user9Id },
                new ProjectTask { Id = task27Id, ProjectTaskTitle = "Security Audit Remediation", ProjectTaskDescription = "Remediate issues found in audit.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project9Id, AssignedUserId = user10Id },
                new ProjectTask { Id = task28Id, ProjectTaskTitle = "HR System Training", ProjectTaskDescription = "Train staff on new HR system.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project10Id, AssignedUserId = user1Id },
                new ProjectTask { Id = task29Id, ProjectTaskTitle = "E-commerce SEO Audit", ProjectTaskDescription = "Audit SEO for e-commerce platform.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project11Id, AssignedUserId = user2Id },
                new ProjectTask { Id = task30Id, ProjectTaskTitle = "Chatbot Integration Testing", ProjectTaskDescription = "Test chatbot integration with support system.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project12Id, AssignedUserId = user3Id },
                new ProjectTask { Id = task31Id, ProjectTaskTitle = "Marketing Campaign Launch", ProjectTaskDescription = "Launch new marketing campaign.", ProjectTaskStatus = TaskProgressStatus.ToDo, ProjectId = project13Id, AssignedUserId = user4Id },
                new ProjectTask { Id = task32Id, ProjectTaskTitle = "Inventory System Testing", ProjectTaskDescription = "Test inventory optimization system.", ProjectTaskStatus = TaskProgressStatus.InProgress, ProjectId = project14Id, AssignedUserId = user5Id }
            );

        }
    }
}
