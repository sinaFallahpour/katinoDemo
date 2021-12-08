using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addingUsedPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsedFreePlan",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UsedPlans",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsedPlans",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsUsedFreePlan",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
