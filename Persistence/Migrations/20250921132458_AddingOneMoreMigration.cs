using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddingOneMoreMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ProjectDescription", "ProjectName" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"), "Revamp the mobile app UI/UX for better engagement and retention.", "Mobile App Redesign" },
                    { new Guid("a3b4c5d6-e7f8-6a9b-0c1d-2e3f4a5b6c7d"), "Enable secure remote work for all employees.", "Remote Work Enablement" },
                    { new Guid("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d"), "Conduct a comprehensive security audit of all systems.", "Security Audit" },
                    { new Guid("b2c3d4e5-f6a7-5b8c-9d0e-1f2a3b4c5d6e"), "Migrate legacy systems to cloud infrastructure for scalability.", "Cloud Migration" },
                    { new Guid("b4c5d6e7-f8a9-7b0c-1d2e-3f4a5b6c7d8e"), "Develop a tool to track regulatory compliance tasks.", "Compliance Tracker" },
                    { new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e"), "Upgrade HR software for improved payroll and benefits management.", "HR System Upgrade" },
                    { new Guid("c3d4e5f6-a7b8-6c9d-0e1f-2a3b4c5d6e7f"), "Centralize API management and security with a gateway solution.", "API Gateway Implementation" },
                    { new Guid("c5d6e7f8-a9b0-8c1d-2e3f-4a5b6c7d8e9f"), "Create a system for employee performance reviews and feedback.", "Performance Review System" },
                    { new Guid("c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f"), "Expand e-commerce platform to new markets and regions.", "E-commerce Expansion" },
                    { new Guid("d0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a"), "Integrate AI-powered chatbot for customer support.", "AI Chatbot Integration" },
                    { new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a"), "Build a platform for advanced business intelligence and analytics.", "Data Analytics Platform" },
                    { new Guid("d6e7f8a9-b0c1-9d2e-3f4a-5b6c7d8e9f0a"), "Streamline supplier onboarding and management processes.", "Supplier Management" },
                    { new Guid("e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b"), "Implement marketing automation tools for campaigns.", "Marketing Automation" },
                    { new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b"), "Develop a self-service portal for customers to manage accounts.", "Customer Portal Launch" },
                    { new Guid("e7f8a9b0-c1d2-0e3f-4a5b-6c7d8e9f0a1b"), "Build an internal knowledge base for documentation and FAQs.", "Knowledge Base" },
                    { new Guid("f2a3b4c5-d6e7-5f8a-9b0c-1d2e3f4a5b6c"), "Optimize inventory management using predictive analytics.", "Inventory Optimization" },
                    { new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c"), "Automate CI/CD pipelines and infrastructure provisioning.", "DevOps Automation" },
                    { new Guid("f8a9b0c1-d2e3-1f4a-5b6c-7d8e9f0a1b2c"), "Plan and document incident response procedures for IT.", "Incident Response Planning" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "UserEmail", "UserName" },
                values: new object[,]
                {
                    { new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f"), "", "sofia.ivanova@example.org", "Sofia Ivanova" },
                    { new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f"), "", "fatima.alfarsi@example.org", "Fatima Al-Farsi" },
                    { new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a"), "", "james.oconnor@example.com", "James O'Connor" },
                    { new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a"), "", "david.kim@example.com", "David Kim" },
                    { new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b"), "", "aisha.patel@example.net", "Aisha Patel" },
                    { new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c"), "", "carlos.mendes@example.org", "Carlos Mendes" },
                    { new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d"), "", "emma.johansson@example.com", "Emma Johansson" },
                    { new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e"), "", "chen.wei@example.net", "Chen Wei" }
                });

            migrationBuilder.InsertData(
                table: "ProjectTasks",
                columns: new[] { "Id", "AssignedUserId", "ProjectId", "ProjectTaskDescription", "ProjectTaskStatus", "ProjectTaskTitle" },
                values: new object[,]
                {
                    { new Guid("a1e2f3d4-1a2b-4c3d-9e1f-1a2b3c4d5e6f"), new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f"), new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"), "Create wireframes for the new mobile app redesign.", 0, "Design Mobile App Wireframes" },
                    { new Guid("a2b3c4d5-2a3b-4c5d-6e7f-8a9b0c1d2e3f"), new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a"), new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"), "Test mobile app features and report bugs.", 0, "Mobile App Testing" },
                    { new Guid("a3c5f6d7-3f4a-7b6c-9e3f-3f4a5b6c7d8e"), new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b"), new Guid("a3b4c5d6-e7f8-6a9b-0c1d-2e3f4a5b6c7d"), "Draft remote work policy for employees.", 0, "Remote Work Policy Draft" },
                    { new Guid("a7c8f9d0-7a8b-1c9d-3e7f-7a8b9c1d2e3f"), new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f"), new Guid("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d"), "Prepare checklist for security audit.", 0, "Security Audit Checklist" },
                    { new Guid("a8b9c0d1-8a9b-0c1d-2e3f-4a5b6c7d8e9f"), new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a"), new Guid("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d"), "Remediate issues found in audit.", 0, "Security Audit Remediation" },
                    { new Guid("b2f3a4e5-2b3c-5d4e-8f2a-2b3c4d5e6f7a"), new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a"), new Guid("b2c3d4e5-f6a7-5b8c-9d0e-1f2a3b4c5d6e"), "Draft migration plan for legacy systems.", 1, "Cloud Migration Planning" },
                    { new Guid("b3c4d5e6-3b4c-5d6e-7f8a-9b0c1d2e3f4a"), new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b"), new Guid("b2c3d4e5-f6a7-5b8c-9d0e-1f2a3b4c5d6e"), "Execute migration of selected services.", 1, "Cloud Migration Execution" },
                    { new Guid("b4d6a7e8-4a5b-8c7d-1f4a-4a5b6c7d8e9f"), new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c"), new Guid("b4c5d6e7-f8a9-7b0c-1d2e-3f4a5b6c7d8e"), "Design UI for compliance tracker tool.", 1, "Compliance Tracker UI" },
                    { new Guid("b8d9a1e2-8b9c-2d1e-4f8a-8b9c1d2e3f4a"), new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a"), new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e"), "Migrate data to new HR system.", 1, "HR System Data Migration" },
                    { new Guid("b9c0d1e2-9b0c-1d2e-3f4a-5b6c7d8e9f0a"), new Guid("be2f07b8-c92d-4138-b945-8fefecbc3cbe"), new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e"), "Train staff on new HR system.", 1, "HR System Training" },
                    { new Guid("c0d1e2f3-0c1d-2e3f-4a5b-6c7d8e9f0a1b"), new Guid("6c8cd30a-cca3-4537-a8dd-7b6ac903bf29"), new Guid("c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f"), "Audit SEO for e-commerce platform.", 0, "E-commerce SEO Audit" },
                    { new Guid("c3e4b5f6-3c4d-6e5f-7a3b-3c4d5e6f7a8b"), new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b"), new Guid("c3d4e5f6-a7b8-6c9d-0e1f-2a3b4c5d6e7f"), "Set up API gateway and configure endpoints.", 0, "API Gateway Setup" },
                    { new Guid("c4d5e6f7-4c5d-6e7f-8a9b-0c1d2e3f4a5b"), new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c"), new Guid("c3d4e5f6-a7b8-6c9d-0e1f-2a3b4c5d6e7f"), "Monitor API gateway traffic and errors.", 0, "API Gateway Monitoring" },
                    { new Guid("c5e7b8f9-5b6c-9d8e-2a5b-5b6c7d8e9f0a"), new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d"), new Guid("c5d6e7f8-a9b0-8c1d-2e3f-4a5b6c7d8e9f"), "Create template for performance reviews.", 0, "Performance Review Template" },
                    { new Guid("c9e1b2f3-9c1d-3e2f-5a9b-9c1d2e3f4a5b"), new Guid("be2f07b8-c92d-4138-b945-8fefecbc3cbe"), new Guid("c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f"), "Analyze new market opportunities for e-commerce.", 0, "E-commerce Market Analysis" },
                    { new Guid("d0f2c3a4-0c1d-4e3f-6b0c-0c1d2e3f4a5b"), new Guid("6c8cd30a-cca3-4537-a8dd-7b6ac903bf29"), new Guid("d0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a"), "Prepare training data for AI chatbot.", 1, "Chatbot Training Data" },
                    { new Guid("d1e2f3a4-1d2e-3f4a-5b6c-7d8e9f0a1b2c"), new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f"), new Guid("d0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a"), "Test chatbot integration with support system.", 1, "Chatbot Integration Testing" },
                    { new Guid("d4f5c6a7-4d5e-7f6a-9b4c-4d5e6f7a8b9c"), new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c"), new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a"), "Develop dashboard for analytics platform.", 1, "Analytics Dashboard Development" },
                    { new Guid("d5e6f7a8-5d6e-7f8a-9b0c-1d2e3f4a5b6c"), new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d"), new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a"), "Validate analytics data sources.", 1, "Analytics Data Validation" },
                    { new Guid("d6f8c9a1-6c7d-0e9f-3b6c-6c7d8e9f0a1b"), new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e"), new Guid("d6e7f8a9-b0c1-9d2e-3f4a-5b6c7d8e9f0a"), "Prepare onboarding documentation for suppliers.", 1, "Supplier Onboarding Docs" },
                    { new Guid("e1a3d4b5-1d2e-5f4a-7c1d-1d2e3f4a5b6c"), new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f"), new Guid("e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b"), "Set up marketing automation tools.", 0, "Marketing Automation Setup" },
                    { new Guid("e2f3a4b5-2e3f-4a5b-6c7d-8e9f0a1b2c3d"), new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a"), new Guid("e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b"), "Launch new marketing campaign.", 0, "Marketing Campaign Launch" },
                    { new Guid("e5a6d7b8-5e6f-8a7b-1c5d-5e6f7a8b9c1d"), new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d"), new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b"), "Implement authentication for customer portal.", 0, "Customer Portal Authentication" },
                    { new Guid("e6f7a8b9-6e7f-8a9b-0c1d-2e3f4a5b6c7d"), new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e"), new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b"), "Collect feedback from portal users.", 0, "Customer Portal Feedback" },
                    { new Guid("e7a9d0b2-7d8e-1f0a-4c7d-7d8e9f0a1b2c"), new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f"), new Guid("e7f8a9b0-c1d2-0e3f-4a5b-6c7d8e9f0a1b"), "Define structure for internal knowledge base.", 0, "Knowledge Base Structure" },
                    { new Guid("f2b4e5c6-2e3f-6a5b-8d2e-2e3f4a5b6c7d"), new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a"), new Guid("f2a3b4c5-d6e7-5f8a-9b0c-1d2e3f4a5b6c"), "Import inventory data for optimization.", 1, "Inventory Data Import" },
                    { new Guid("f3a4b5c6-3f4a-5b6c-7d8e-9f0a1b2c3d4e"), new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b"), new Guid("f2a3b4c5-d6e7-5f8a-9b0c-1d2e3f4a5b6c"), "Test inventory optimization system.", 1, "Inventory System Testing" },
                    { new Guid("f6b7e8c9-6f7a-9b8c-2d6e-6f7a8b9c1d2e"), new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e"), new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c"), "Automate CI/CD pipeline for deployments.", 1, "DevOps Pipeline Automation" },
                    { new Guid("f7a8b9c0-7f8a-9b0c-1d2e-3f4a5b6c7d8e"), new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f"), new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c"), "Review DevOps tools for automation.", 1, "DevOps Tooling Review" },
                    { new Guid("f8b0e1c3-8e9f-2a1b-5d8e-8e9f0a1b2c3d"), new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a"), new Guid("f8a9b0c1-d2e3-1f4a-5b6c-7d8e9f0a1b2c"), "Draft playbook for incident response.", 1, "Incident Response Playbook" }
                });

            migrationBuilder.InsertData(
                table: "ProjectUsers",
                columns: new[] { "ProjectId", "UserId", "Role" },
                values: new object[,]
                {
                    { new Guid("37eac064-228b-45d8-bc19-9d09b94067b9"), new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f"), 1 },
                    { new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"), new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f"), 0 },
                    { new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"), new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a"), 1 },
                    { new Guid("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d"), new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f"), 0 },
                    { new Guid("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d"), new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a"), 1 },
                    { new Guid("b2c3d4e5-f6a7-5b8c-9d0e-1f2a3b4c5d6e"), new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a"), 0 },
                    { new Guid("b2c3d4e5-f6a7-5b8c-9d0e-1f2a3b4c5d6e"), new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b"), 1 },
                    { new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e"), new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a"), 0 },
                    { new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e"), new Guid("be2f07b8-c92d-4138-b945-8fefecbc3cbe"), 1 },
                    { new Guid("c3d4e5f6-a7b8-6c9d-0e1f-2a3b4c5d6e7f"), new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b"), 0 },
                    { new Guid("c3d4e5f6-a7b8-6c9d-0e1f-2a3b4c5d6e7f"), new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c"), 1 },
                    { new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a"), new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c"), 0 },
                    { new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a"), new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d"), 1 },
                    { new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b"), new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d"), 0 },
                    { new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b"), new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e"), 1 },
                    { new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c"), new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f"), 1 },
                    { new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c"), new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e"), 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("a1e2f3d4-1a2b-4c3d-9e1f-1a2b3c4d5e6f"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("a2b3c4d5-2a3b-4c5d-6e7f-8a9b0c1d2e3f"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("a3c5f6d7-3f4a-7b6c-9e3f-3f4a5b6c7d8e"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("a7c8f9d0-7a8b-1c9d-3e7f-7a8b9c1d2e3f"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("a8b9c0d1-8a9b-0c1d-2e3f-4a5b6c7d8e9f"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("b2f3a4e5-2b3c-5d4e-8f2a-2b3c4d5e6f7a"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("b3c4d5e6-3b4c-5d6e-7f8a-9b0c1d2e3f4a"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("b4d6a7e8-4a5b-8c7d-1f4a-4a5b6c7d8e9f"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("b8d9a1e2-8b9c-2d1e-4f8a-8b9c1d2e3f4a"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("b9c0d1e2-9b0c-1d2e-3f4a-5b6c7d8e9f0a"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("c0d1e2f3-0c1d-2e3f-4a5b-6c7d8e9f0a1b"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("c3e4b5f6-3c4d-6e5f-7a3b-3c4d5e6f7a8b"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("c4d5e6f7-4c5d-6e7f-8a9b-0c1d2e3f4a5b"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("c5e7b8f9-5b6c-9d8e-2a5b-5b6c7d8e9f0a"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("c9e1b2f3-9c1d-3e2f-5a9b-9c1d2e3f4a5b"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("d0f2c3a4-0c1d-4e3f-6b0c-0c1d2e3f4a5b"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-1d2e-3f4a-5b6c-7d8e9f0a1b2c"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("d4f5c6a7-4d5e-7f6a-9b4c-4d5e6f7a8b9c"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("d5e6f7a8-5d6e-7f8a-9b0c-1d2e3f4a5b6c"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("d6f8c9a1-6c7d-0e9f-3b6c-6c7d8e9f0a1b"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("e1a3d4b5-1d2e-5f4a-7c1d-1d2e3f4a5b6c"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("e2f3a4b5-2e3f-4a5b-6c7d-8e9f0a1b2c3d"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("e5a6d7b8-5e6f-8a7b-1c5d-5e6f7a8b9c1d"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("e6f7a8b9-6e7f-8a9b-0c1d-2e3f4a5b6c7d"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("e7a9d0b2-7d8e-1f0a-4c7d-7d8e9f0a1b2c"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("f2b4e5c6-2e3f-6a5b-8d2e-2e3f4a5b6c7d"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("f3a4b5c6-3f4a-5b6c-7d8e-9f0a1b2c3d4e"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("f6b7e8c9-6f7a-9b8c-2d6e-6f7a8b9c1d2e"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("f7a8b9c0-7f8a-9b0c-1d2e-3f4a5b6c7d8e"));

            migrationBuilder.DeleteData(
                table: "ProjectTasks",
                keyColumn: "Id",
                keyValue: new Guid("f8b0e1c3-8e9f-2a1b-5d8e-8e9f0a1b2c3d"));

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("37eac064-228b-45d8-bc19-9d09b94067b9"), new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"), new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"), new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d"), new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d"), new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("b2c3d4e5-f6a7-5b8c-9d0e-1f2a3b4c5d6e"), new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("b2c3d4e5-f6a7-5b8c-9d0e-1f2a3b4c5d6e"), new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e"), new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e"), new Guid("be2f07b8-c92d-4138-b945-8fefecbc3cbe") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("c3d4e5f6-a7b8-6c9d-0e1f-2a3b4c5d6e7f"), new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("c3d4e5f6-a7b8-6c9d-0e1f-2a3b4c5d6e7f"), new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a"), new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a"), new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b"), new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b"), new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c"), new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f") });

            migrationBuilder.DeleteData(
                table: "ProjectUsers",
                keyColumns: new[] { "ProjectId", "UserId" },
                keyValues: new object[] { new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c"), new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e") });

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-6a9b-0c1d-2e3f4a5b6c7d"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-0a3b-4c5d-6e7f8a9b0c1d"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-5b8c-9d0e-1f2a3b4c5d6e"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-7b0c-1d2e-3f4a5b6c7d8e"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-1b4c-5d6e-7f8a9b0c1d2e"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-6c9d-0e1f-2a3b4c5d6e7f"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-8c1d-2e3f-4a5b6c7d8e9f"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-2c5d-6e7f-8a9b0c1d2e3f"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-3d6e-7f8a-9b0c1d2e3f4a"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-7d0e-1f2a-3b4c5d6e7f8a"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-9d2e-3f4a-5b6c7d8e9f0a"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-4e7f-8a9b-0c1d2e3f4a5b"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-8e1f-2a3b-4c5d6e7f8a9b"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-0e3f-4a5b-6c7d8e9f0a1b"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-5f8a-9b0c-1d2e3f4a5b6c"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-9f2a-3b4c-5d6e7f8a9b0c"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-1f4a-5b6c-7d8e9f0a1b2c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1e1c3d2-1b2a-4c3d-9e1f-1a2b3c4d5e6f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a7c7d9e8-7b8a-1c9d-3e7f-7a8b9c1d2e3f"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b2f2d4e3-2c3b-5d4e-8f2a-2b3c4d5e6f7a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b8d8e1f9-8c9b-2d1e-4f8a-8b9c1d2e3f4a"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("c3e3f5a4-3d4c-6e5f-7a3b-3c4d5e6f7a8b"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4f4a6b5-4e5d-7f6a-9b4c-4d5e6f7a8b9c"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e5a5b7c6-5f6e-8a7b-1c5d-5e6f7a8b9c1d"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("f6b6c8d7-6a7f-9b8c-2d6e-6f7a8b9c1d2e"));
        }
    }
}
