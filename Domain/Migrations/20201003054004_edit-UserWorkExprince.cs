using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editUserWorkExprince : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserWorkExperiences_CreateDate",
                table: "UserWorkExperiences",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkExperiences_IsActive",
                table: "UserWorkExperiences",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkExperiences_WorkTitle",
                table: "UserWorkExperiences",
                column: "WorkTitle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserWorkExperiences_CreateDate",
                table: "UserWorkExperiences");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkExperiences_IsActive",
                table: "UserWorkExperiences");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkExperiences_WorkTitle",
                table: "UserWorkExperiences");
        }
    }
}
