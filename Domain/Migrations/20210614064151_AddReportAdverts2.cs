using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class AddReportAdverts2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportAdverts_AspNetUsers_UserId1",
                table: "ReportAdverts");

            migrationBuilder.DropIndex(
                name: "IX_ReportAdverts_UserId1",
                table: "ReportAdverts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ReportAdverts");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ReportAdverts");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ReportAdverts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ReportAdverts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ReportAdverts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ReportAdverts");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ReportAdverts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ReportAdverts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportAdverts_UserId1",
                table: "ReportAdverts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAdverts_AspNetUsers_UserId1",
                table: "ReportAdverts",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
