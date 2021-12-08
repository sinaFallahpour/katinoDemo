using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class ahks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "RefrenceDepositRequest");

            migrationBuilder.AddColumn<string>(
                name: "AdminDescriptions",
                table: "RefrenceDepositRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminDescriptions",
                table: "RefrenceDepositRequest");

            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "RefrenceDepositRequest",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
