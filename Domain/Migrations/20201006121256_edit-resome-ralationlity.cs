using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editresomeralationlity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

        

           

            migrationBuilder.AddColumn<int>(
                name: "EducationalBackgroundId",
                table: "Resomes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Resomes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserJobPreferencesId",
                table: "Resomes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserJobSkillId",
                table: "Resomes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserLanguageId",
                table: "Resomes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserWorkExperienceId",
                table: "Resomes",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
