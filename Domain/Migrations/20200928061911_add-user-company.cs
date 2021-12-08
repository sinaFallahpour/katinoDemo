using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addusercompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EngFullname",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Fullname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EngFullname",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "CompanyEngName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyPersianName",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyEngName",
                table: "AspNetUsers",
                column: "CompanyEngName",
                unique: true,
                filter: "[CompanyEngName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyPersianName",
                table: "AspNetUsers",
                column: "CompanyPersianName",
                unique: true,
                filter: "[CompanyPersianName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Fullname",
                table: "AspNetUsers",
                column: "Fullname");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyEngName",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CompanyPersianName",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Fullname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyEngName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyPersianName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "EngFullname",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EngFullname",
                table: "AspNetUsers",
                column: "EngFullname",
                unique: true,
                filter: "[EngFullname] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Fullname",
                table: "AspNetUsers",
                column: "Fullname",
                unique: true,
                filter: "[Fullname] IS NOT NULL");
        }
    }
}
