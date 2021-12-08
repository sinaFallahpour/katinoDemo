using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class changeresomerelational : Migration
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
                name: "EducationalBackgroundId",
                table: "Resomes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Resomes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserJobPreferencesId",
                table: "Resomes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserJobSkillId",
                table: "Resomes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserLanguageId",
                table: "Resomes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserWorkExperienceId",
                table: "Resomes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "EducationalBackgrounds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_EducationalBackgroundId",
                table: "Resomes",
                column: "EducationalBackgroundId",
                unique: true,
                filter: "[EducationalBackgroundId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_UserJobPreferencesId",
                table: "Resomes",
                column: "UserJobPreferencesId",
                unique: true,
                filter: "[UserJobPreferencesId] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ResomeId",
                table: "AspNetUsers",
                column: "ResomeId",
                unique: true,
                filter: "[ResomeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Resomes_ResomeId",
                table: "AspNetUsers",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_EducationalBackgrounds_EducationalBackgroundId",
                table: "Resomes",
                column: "EducationalBackgroundId",
                principalTable: "EducationalBackgrounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_UserJobPreferences_UserJobPreferencesId",
                table: "Resomes",
                column: "UserJobPreferencesId",
                principalTable: "UserJobPreferences",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Resomes_ResomeId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Resomes_EducationalBackgrounds_EducationalBackgroundId",
                table: "Resomes");

            migrationBuilder.DropForeignKey(
                name: "FK_Resomes_UserJobPreferences_UserJobPreferencesId",
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
                name: "IX_Resomes_UserJobPreferencesId",
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
                name: "EducationalBackgroundId",
                table: "Resomes");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Resomes");

            migrationBuilder.DropColumn(
                name: "UserJobPreferencesId",
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

            migrationBuilder.DropColumn(
                name: "ResomeId",
                table: "EducationalBackgrounds");

            migrationBuilder.DropColumn(
                name: "ResomeId",
                table: "AspNetUsers");
        }
    }
}
