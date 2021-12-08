using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class assFAS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RefrenceDepositRequest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RefrenceDepositRequest",
                type: "integer",
                nullable: true);
        }
    }
}
