using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class chngeTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "TransactionCode",
                table: "Transactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionCode",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
