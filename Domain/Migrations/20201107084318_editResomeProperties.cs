using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editResomeProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resomes_EducationalBackgrounds_EducationalBackgroundId",
                table: "Resomes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resomes_UserJobSkills_UserJobSkillId",
                table: "Resomes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resomes_UserLanguage_UserLanguageId",
                table: "Resomes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resomes_UserWorkExperiences_UserWorkExperienceId",
                table: "Resomes");

            migrationBuilder.DropIndex(
                name: "IX_Resomes_EducationalBackgroundId",
                table: "Resomes");

            migrationBuilder.DropIndex(
                name: "IX_Resomes_UserJobSkillId",
                table: "Resomes");

            migrationBuilder.DropIndex(
                name: "IX_Resomes_UserLanguageId",
                table: "Resomes");

            migrationBuilder.DropIndex(
                name: "IX_Resomes_UserWorkExperienceId",
                table: "Resomes");

            migrationBuilder.DropColumn(
                name: "EducationalBackgroundId",
                table: "Resomes");

            migrationBuilder.DropColumn(
                name: "UserJobSkillId",
                table: "Resomes");

            migrationBuilder.DropColumn(
                name: "UserLanguageId",
                table: "Resomes");

            migrationBuilder.DropColumn(
                name: "UserWorkExperienceId",
                table: "Resomes");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkExperiences_ResomeId",
                table: "UserWorkExperiences",
                column: "ResomeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguage_ResomeId",
                table: "UserLanguage",
                column: "ResomeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobSkills_ResomeId",
                table: "UserJobSkills",
                column: "ResomeId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationalBackgrounds_ResomeId",
                table: "EducationalBackgrounds",
                column: "ResomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationalBackgrounds_Resomes_ResomeId",
                table: "EducationalBackgrounds",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobSkills_Resomes_ResomeId",
                table: "UserJobSkills",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_Resomes_ResomeId",
                table: "UserLanguage",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWorkExperiences_Resomes_ResomeId",
                table: "UserWorkExperiences",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationalBackgrounds_Resomes_ResomeId",
                table: "EducationalBackgrounds");

            migrationBuilder.DropForeignKey(
                name: "FK_UserJobSkills_Resomes_ResomeId",
                table: "UserJobSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_Resomes_ResomeId",
                table: "UserLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWorkExperiences_Resomes_ResomeId",
                table: "UserWorkExperiences");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkExperiences_ResomeId",
                table: "UserWorkExperiences");

            migrationBuilder.DropIndex(
                name: "IX_UserLanguage_ResomeId",
                table: "UserLanguage");

            migrationBuilder.DropIndex(
                name: "IX_UserJobSkills_ResomeId",
                table: "UserJobSkills");

            migrationBuilder.DropIndex(
                name: "IX_EducationalBackgrounds_ResomeId",
                table: "EducationalBackgrounds");

            migrationBuilder.AddColumn<int>(
                name: "EducationalBackgroundId",
                table: "Resomes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserJobSkillId",
                table: "Resomes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserLanguageId",
                table: "Resomes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserWorkExperienceId",
                table: "Resomes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_EducationalBackgroundId",
                table: "Resomes",
                column: "EducationalBackgroundId",
                unique: true,
                filter: "[EducationalBackgroundId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_UserJobSkillId",
                table: "Resomes",
                column: "UserJobSkillId",
                unique: true,
                filter: "[UserJobSkillId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_UserLanguageId",
                table: "Resomes",
                column: "UserLanguageId",
                unique: true,
                filter: "[UserLanguageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_UserWorkExperienceId",
                table: "Resomes",
                column: "UserWorkExperienceId",
                unique: true,
                filter: "[UserWorkExperienceId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_EducationalBackgrounds_EducationalBackgroundId",
                table: "Resomes",
                column: "EducationalBackgroundId",
                principalTable: "EducationalBackgrounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_UserJobSkills_UserJobSkillId",
                table: "Resomes",
                column: "UserJobSkillId",
                principalTable: "UserJobSkills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_UserLanguage_UserLanguageId",
                table: "Resomes",
                column: "UserLanguageId",
                principalTable: "UserLanguage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_UserWorkExperiences_UserWorkExperienceId",
                table: "Resomes",
                column: "UserWorkExperienceId",
                principalTable: "UserWorkExperiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
