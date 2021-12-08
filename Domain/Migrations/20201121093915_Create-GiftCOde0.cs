using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class CreateGiftCOde0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCart_AspNetUsers_EmployerId",
                table: "GiftCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GiftCart",
                table: "GiftCart");

            migrationBuilder.RenameTable(
                name: "GiftCart",
                newName: "GiftCarts");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCart_GiftCode",
                table: "GiftCarts",
                newName: "IX_GiftCarts_GiftCode");

            migrationBuilder.RenameIndex(
                name: "IX_GiftCart_EmployerId",
                table: "GiftCarts",
                newName: "IX_GiftCarts_EmployerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GiftCarts",
                table: "GiftCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GiftCarts_AspNetUsers_EmployerId",
                table: "GiftCarts",
                column: "EmployerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
