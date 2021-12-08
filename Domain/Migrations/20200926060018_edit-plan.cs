using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Plan_PlanId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PlanId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "AspNetUsers");
        }
    }
}
