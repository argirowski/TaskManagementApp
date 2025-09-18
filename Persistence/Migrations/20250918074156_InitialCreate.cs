using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectTaskTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectTaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectTaskStatus = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTasks_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "ProjectDescription", "ProjectName" },
                values: new object[,]
                {
                    { new Guid("15d4a262-aac1-418b-b32e-0bffd37966d5"), "Venison cow kielbasa porchetta tri-tip drumstick alcatra capicola. Cow tail alcatra swine frankfurter beef, meatball pork loin short ribs chislic chuck doner shank pig ham.", "Onboarding UX Refresh" },
                    { new Guid("37eac064-228b-45d8-bc19-9d09b94067b9"), "Kielbasa drumstick frankfurter corned beef andouille prosciutto hamburger pork chop. Meatball brisket turducken t-bone pancetta.", "Process Sync Initiative" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "UserEmail", "UserName" },
                values: new object[,]
                {
                    { new Guid("6c8cd30a-cca3-4537-a8dd-7b6ac903bf29"), "", "luca.moretti@example.net", "Luca Moretti" },
                    { new Guid("be2f07b8-c92d-4138-b945-8fefecbc3cbe"), "", "maya.ellington@example.com", "Maya Ellington" }
                });

            migrationBuilder.InsertData(
                table: "ProjectTasks",
                columns: new[] { "Id", "AssignedUserId", "ProjectId", "ProjectTaskDescription", "ProjectTaskStatus", "ProjectTaskTitle" },
                values: new object[,]
                {
                    { new Guid("724981b4-fe11-4247-8a51-2fdb56d16a50"), new Guid("be2f07b8-c92d-4138-b945-8fefecbc3cbe"), new Guid("37eac064-228b-45d8-bc19-9d09b94067b9"), "Kielbasa beef tenderloin pastrami drumstick fatback pork chop. Short ribs ground round brisket, jerky pork chop porchetta biltong pancetta ham hock.", 1, "Conduct Vendor Risk Assessment" },
                    { new Guid("e79e1167-8ba5-4c8e-afe9-9b3dcf69df2a"), new Guid("6c8cd30a-cca3-4537-a8dd-7b6ac903bf29"), new Guid("15d4a262-aac1-418b-b32e-0bffd37966d5"), "Ham hock sirloin turducken venison. Picanha pork landjaeger, capicola meatloaf chuck pastrami ham hock. Alcatra ribeye frankfurter drumstick pork. ", 0, "- Audit Data Access Permissions\r\n" }
                });

            migrationBuilder.InsertData(
                table: "ProjectUsers",
                columns: new[] { "ProjectId", "UserId", "Role" },
                values: new object[,]
                {
                    { new Guid("15d4a262-aac1-418b-b32e-0bffd37966d5"), new Guid("6c8cd30a-cca3-4537-a8dd-7b6ac903bf29"), 1 },
                    { new Guid("15d4a262-aac1-418b-b32e-0bffd37966d5"), new Guid("be2f07b8-c92d-4138-b945-8fefecbc3cbe"), 0 },
                    { new Guid("37eac064-228b-45d8-bc19-9d09b94067b9"), new Guid("6c8cd30a-cca3-4537-a8dd-7b6ac903bf29"), 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AssignedUserId",
                table: "ProjectTasks",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectId",
                table: "ProjectTasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTasks");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
