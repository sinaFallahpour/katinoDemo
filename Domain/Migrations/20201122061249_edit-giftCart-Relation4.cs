using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editgiftCartRelation4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "GiftCart_Copys",
                newName: "GiftCarts");

          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiftCarts_AspNetUsers_UserId",
                table: "GiftCarts");

            migrationBuilder.DropIndex(
                name: "IX_GiftCarts_UserId",
                table: "GiftCarts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GiftCarts");

            migrationBuilder.CreateTable(
                name: "GiftCart_Copys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    EmployerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GiftCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    IsUse = table.Column<bool>(type: "bit", nullable: false),
                    UseAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftCart_Copys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftCart_Copys_AspNetUsers_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiftCart_Copys_EmployerId",
                table: "GiftCart_Copys",
                column: "EmployerId");
        }
    }
}
