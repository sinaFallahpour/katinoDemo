using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class CreateGiftCOde67 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "GiftCart_Copy",
                newName: "GiftCarts");
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiftCart_Copy",
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
                    table.PrimaryKey("PK_GiftCart_Copy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftCart_Copy_AspNetUsers_EmployerId",
                        column: x => x.EmployerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GiftCart_Copy_EmployerId",
                table: "GiftCart_Copy",
                column: "EmployerId",
                unique: true,
                filter: "[EmployerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_GiftCart_Copy_GiftCode",
                table: "GiftCart_Copy",
                column: "GiftCode");
        }
    }
}
