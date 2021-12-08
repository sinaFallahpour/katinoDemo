using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addloging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExceptionMessage = table.Column<string>(nullable: true),
                    ExceptionType = table.Column<string>(nullable: true),
                    MethodName = table.Column<string>(nullable: true),
                    TableName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Date",
                table: "Logs",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_ExceptionMessage",
                table: "Logs",
                column: "ExceptionMessage");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_MethodName",
                table: "Logs",
                column: "MethodName");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_TableName",
                table: "Logs",
                column: "TableName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
