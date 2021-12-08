using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editsomeindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EducationalBackgrounds_CreateDate",
                table: "EducationalBackgrounds",
                column: "CreateDate");

            migrationBuilder.CreateIndex(
                name: "IX_EducationalBackgrounds_IsActive",
                table: "EducationalBackgrounds",
                column: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EducationalBackgrounds_CreateDate",
                table: "EducationalBackgrounds");

            migrationBuilder.DropIndex(
                name: "IX_EducationalBackgrounds_IsActive",
                table: "EducationalBackgrounds");
        }
    }
}
