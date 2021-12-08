using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class fdsfs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSkills_Categories_CategoryId",
                table: "JobSkills");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "JobSkills",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkills_Categories_CategoryId",
                table: "JobSkills",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSkills_Categories_CategoryId",
                table: "JobSkills");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "JobSkills",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkills_Categories_CategoryId",
                table: "JobSkills",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
