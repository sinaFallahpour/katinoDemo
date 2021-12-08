using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class adduseraddSpecialUserType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpecialEmpolyee",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialEmpolyee",
                table: "AspNetUsers");
        }
    }
}
