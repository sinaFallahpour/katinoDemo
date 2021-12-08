using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class createEducationBackgroundDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationalBackgrounds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldOfStudy = table.Column<string>(maxLength: 100, nullable: false),
                    UniversityName = table.Column<string>(maxLength: 100, nullable: false),
                    DegreeOfEducation = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationalBackgrounds", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EducationalBackgrounds_FieldOfStudy",
                table: "EducationalBackgrounds",
                column: "FieldOfStudy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EducationalBackgrounds");
        }
    }
}
