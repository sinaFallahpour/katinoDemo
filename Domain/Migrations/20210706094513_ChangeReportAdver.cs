using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class ChangeReportAdver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "ReportAdverts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ReportAdverts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportAdverts_UserId",
                table: "ReportAdverts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAdverts_AspNetUsers_UserId",
                table: "ReportAdverts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportAdverts_AspNetUsers_UserId",
                table: "ReportAdverts");

            migrationBuilder.DropIndex(
                name: "IX_ReportAdverts_UserId",
                table: "ReportAdverts");

            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "ReportAdverts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReportAdverts");
        }
    }
}
