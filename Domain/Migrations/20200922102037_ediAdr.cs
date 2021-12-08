using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class ediAdr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionOfCompany",
                table: "JobAdvertisements");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "JobAdvertisements",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "JobAdvertisements",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "JobAdvertisements",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_CompanyId",
                table: "JobAdvertisements",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisements_AspNetUsers_CompanyId",
                table: "JobAdvertisements",
                column: "CompanyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisements_AspNetUsers_CompanyId",
                table: "JobAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_JobAdvertisements_CompanyId",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "JobAdvertisements");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionOfCompany",
                table: "JobAdvertisements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
