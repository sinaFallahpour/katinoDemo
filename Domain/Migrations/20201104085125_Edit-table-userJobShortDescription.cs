using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class EdittableuserJobShortDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCompanies",
                table: "UserJobShortDescription");

            migrationBuilder.DropColumn(
                name: "LastEducationBackgrounds",
                table: "UserJobShortDescription");

            migrationBuilder.AddColumn<int>(
                name: "EmploymentStatus",
                table: "UserJobShortDescription",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentStatus",
                table: "UserJobShortDescription");

            migrationBuilder.AddColumn<string>(
                name: "LastCompanies",
                table: "UserJobShortDescription",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastEducationBackgrounds",
                table: "UserJobShortDescription",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
