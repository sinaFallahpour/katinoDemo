using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class asas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dscsd",
                table: "RefrenceDepositRequest");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "dscsd",
                table: "RefrenceDepositRequest",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
