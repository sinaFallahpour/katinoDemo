using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class edituserlnuage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserLanguage_CreateDate",
                table: "UserLanguage",
                column: "CreateDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserLanguage_CreateDate",
                table: "UserLanguage");
        }
    }
}
