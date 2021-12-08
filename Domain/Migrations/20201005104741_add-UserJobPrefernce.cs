using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addUserJobPrefernce : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserJobPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(maxLength: 100, nullable: false),
                    TypeOfCooperation = table.Column<int>(nullable: false),
                    Senioritylevel = table.Column<int>(nullable: false),
                    Salary = table.Column<int>(nullable: false),
                    Promotion = table.Column<bool>(nullable: false,defaultValue:false),
                    Insurance = table.Column<bool>(nullable: false, defaultValue: false),
                    EducationCourses = table.Column<bool>(nullable: false, defaultValue: false),
                    FlexibleWorkingTime = table.Column<bool>(nullable: false, defaultValue: false),
                    HasMeel = table.Column<bool>(nullable: false, defaultValue: false),
                    TransportationService = table.Column<bool>(nullable: false, defaultValue: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserJobPreferences_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferences_CategoryId",
                table: "UserJobPreferences",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferences_City",
                table: "UserJobPreferences",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferences_CreateDate",
                table: "UserJobPreferences",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferences_Insurance",
                table: "UserJobPreferences",
                column: "Insurance");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferences_Salary",
                table: "UserJobPreferences",
                column: "Salary");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferences_City_Salary_CategoryId",
                table: "UserJobPreferences",
                columns: new[] { "City", "Salary", "CategoryId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserJobPreferences");
        }
    }
}
