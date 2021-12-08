using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addresomeindb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "UserWorkExperiences",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "UserLanguage",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "UserJobSkills",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "UserJobPreferences",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "EducationalBackgrounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "AspNetUsers",
                nullable: true,
                defaultValue:null);

            migrationBuilder.CreateTable(
                name: "Resomes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    AboutMe = table.Column<string>(maxLength: 800, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resomes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkExperien__ResomeId",
                table: "UserWorkExperiences",
                column: "ResomeId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguag__ResomeId",
                table: "UserLanguage",
                column: "ResomeId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserJobSkill__ResomeId",
                table: "UserJobSkills",
                column: "ResomeId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreference__ResomeId",
                table: "UserJobPreferences",
                column: "ResomeId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_EducationalBackground__ResomeId",
                table: "EducationalBackgrounds",
                column: "ResomeId",
                unique: false);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUser__ResomeId",
                table: "AspNetUsers",
                column: "ResomeId",
                unique: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers__Resome_ResomeId",
                table: "AspNetUsers",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EducationalBackground_Resomes_ResomeId",
                table: "EducationalBackgrounds",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobPreference_Resomes_ResomeId",
                table: "UserJobPreferences",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobSkill_Resomes_ResomeId",
                table: "UserJobSkills",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguag_Resomes_ResomeId",
                table: "UserLanguage",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkExperience_Resomes_ResomeId",
                table: "UserWorkExperiences",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Resomes_ResomeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_EducationalBackgrounds_Resomes_ResomeId",
                table: "EducationalBackgrounds");

            migrationBuilder.DropForeignKey(
                name: "FK_UserJobPreferences_Resomes_ResomeId",
                table: "UserJobPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_UserJobSkills_Resomes_ResomeId",
                table: "UserJobSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_Resomes_ResomeId",
                table: "UserLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkExperiences_Resomes_ResomeId",
                table: "UserWorkExperiences");

            migrationBuilder.DropTable(
                name: "Resomes");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkExperien_ResomeId",
                table: "UserWorkExperiences");

            migrationBuilder.DropIndex(
                name: "IX_UserLanguage_ResomeId",
                table: "UserLanguage");

            migrationBuilder.DropIndex(
                name: "IX_UserJobSkills_ResomeId",
                table: "UserJobSkills");

            migrationBuilder.DropIndex(
                name: "IX_UserJobPreferences_ResomeId",
                table: "UserJobPreferences");

            migrationBuilder.DropIndex(
                name: "IX_EducationalBackgrounds_ResomeId",
                table: "EducationalBackgrounds");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ResomeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResomeId",
                table: "UserWorkExperiences");

            migrationBuilder.DropColumn(
                name: "ResomeId",
                table: "UserLanguage");

            migrationBuilder.DropColumn(
                name: "ResomeId",
                table: "UserJobSkills");

            migrationBuilder.DropColumn(
                name: "ResomeId",
                table: "UserJobPreferences");

            migrationBuilder.DropColumn(
                name: "ResomeId",
                table: "EducationalBackgrounds");

            migrationBuilder.DropColumn(
                name: "ResomeId",
                table: "AspNetUsers");
        }
    }
}
