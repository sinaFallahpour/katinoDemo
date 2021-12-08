using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editgiftCartRelation2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GiftCart_Copys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GiftCode = table.Column<string>(maxLength: 5, nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    UseAt = table.Column<DateTime>(nullable: true),
                    ExpireTime = table.Column<DateTime>(nullable: false),
                    IsUse = table.Column<bool>(nullable: false),
                    EmployerId = table.Column<string>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GiftCart_Copys");
        }
    }
}
