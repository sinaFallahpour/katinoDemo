using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RefrenceDepositRequest_UserId",
                table: "RefrenceDepositRequest",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefrenceDepositRequest_UserId",
                table: "RefrenceDepositRequest");
        }
    }
}
