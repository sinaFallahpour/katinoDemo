using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class setUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EngFullname",
                table: "AspNetUsers");

        

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EngFullname",
                table: "AspNetUsers",
                column: "EngFullname",
                unique: true,
                filter: "[EngFullname] IS NOT NULL");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EngFullname",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Fullname",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EngFullname",
                table: "AspNetUsers",
                column: "EngFullname");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Fullname",
                table: "AspNetUsers",
                column: "Fullname");
        }
    }
}
