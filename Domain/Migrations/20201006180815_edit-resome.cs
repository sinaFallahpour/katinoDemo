using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editresome : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

          }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "UserWorkExperiences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "UserLanguage",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "UserJobSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "UserJobPreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EducationalBackgroundId",
                table: "Resomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Resomes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserJobPreferencesId",
                table: "Resomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserJobSkillId",
                table: "Resomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserLanguageId",
                table: "Resomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserWorkExperienceId",
                table: "Resomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "EducationalBackgrounds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResomeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_EducationalBackgroundId",
                table: "Resomes",
                column: "EducationalBackgroundId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_UserJobPreferencesId",
                table: "Resomes",
                column: "UserJobPreferencesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_UserJobSkillId",
                table: "Resomes",
                column: "UserJobSkillId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_UserLanguageId",
                table: "Resomes",
                column: "UserLanguageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resomes_UserWorkExperienceId",
                table: "Resomes",
                column: "UserWorkExperienceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ResomeId",
                table: "AspNetUsers",
                column: "ResomeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Resomes_ResomeId",
                table: "AspNetUsers",
                column: "ResomeId",
                principalTable: "Resomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_EducationalBackgrounds_EducationalBackgroundId",
                table: "Resomes",
                column: "EducationalBackgroundId",
                principalTable: "EducationalBackgrounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_UserJobPreferences_UserJobPreferencesId",
                table: "Resomes",
                column: "UserJobPreferencesId",
                principalTable: "UserJobPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_UserJobSkills_UserJobSkillId",
                table: "Resomes",
                column: "UserJobSkillId",
                principalTable: "UserJobSkills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_UserLanguage_UserLanguageId",
                table: "Resomes",
                column: "UserLanguageId",
                principalTable: "UserLanguage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resomes_UserWorkExperiences_UserWorkExperienceId",
                table: "Resomes",
                column: "UserWorkExperienceId",
                principalTable: "UserWorkExperiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
