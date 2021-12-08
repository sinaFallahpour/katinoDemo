using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editUserForExemptionExpirationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExemptionExpirestionDate",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExemptionExpirestionRecieveDate",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExemptionExpirestionDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExemptionExpirestionRecieveDate",
                table: "AspNetUsers");
        }
    }
}
