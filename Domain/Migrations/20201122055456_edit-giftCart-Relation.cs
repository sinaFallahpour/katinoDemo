using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editgiftCartRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GiftCarts_EmployerId",
                table: "GiftCarts");

            migrationBuilder.CreateIndex(
                name: "IX_GiftCarts_EmployerId",
                table: "GiftCarts",
                column: "EmployerId",
                unique: true,
                filter: "[EmployerId] IS NOT NULL");
        }
    }
}
