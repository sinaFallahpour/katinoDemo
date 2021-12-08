using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editjobskill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "JobSkills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "JobSkills",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "JobSkills",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_JobSkills_CategoryId",
                table: "JobSkills",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobSkills_Categories_CategoryId",
                table: "JobSkills",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobSkills_Categories_CategoryId",
                table: "JobSkills");

            migrationBuilder.DropIndex(
                name: "IX_JobSkills_CategoryId",
                table: "JobSkills");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "JobSkills");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "JobSkills");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "JobSkills");
        }
    }
}
