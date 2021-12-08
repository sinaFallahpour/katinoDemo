using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class adduserjobprefrencecategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserJobPreferences_Categories_CategoryId",
                table: "UserJobPreferences");

            migrationBuilder.DropIndex(
                name: "IX_UserJobPreferences_CategoryId",
                table: "UserJobPreferences");

            migrationBuilder.DropIndex(
                name: "IX_UserJobPreferences_City_Salary_CategoryId",
                table: "UserJobPreferences");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "UserJobPreferences");

            migrationBuilder.CreateTable(
                name: "UserJobPreferenceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    UserJobPreferenceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobPreferenceCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserJobPreferenceCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate:ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserJobPreferenceCategories_UserJobPreferences_UserJobPreferenceId",
                        column: x => x.UserJobPreferenceId,
                        principalTable: "UserJobPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate:ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferences_City_Salary",
                table: "UserJobPreferences",
                columns: new[] { "City", "Salary" });

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferenceCategories_CategoryId",
                table: "UserJobPreferenceCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferenceCategories_UserJobPreferenceId",
                table: "UserJobPreferenceCategories",
                column: "UserJobPreferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserJobPreferenceCategories");

            migrationBuilder.DropIndex(
                name: "IX_UserJobPreferences_City_Salary",
                table: "UserJobPreferences");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "UserJobPreferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferences_CategoryId",
                table: "UserJobPreferences",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJobPreferences_City_Salary_CategoryId",
                table: "UserJobPreferences",
                columns: new[] { "City", "Salary", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserJobPreferences_Categories_CategoryId",
                table: "UserJobPreferences",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
