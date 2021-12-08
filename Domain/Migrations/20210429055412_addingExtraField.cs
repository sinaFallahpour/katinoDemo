using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class addingExtraField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        
            migrationBuilder.AddColumn<string>(
                name: "Descriptions",
                table: "RefrenceDepositRequest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefrenceDescription",
                table: "RefrenceDepositRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriptions",
                table: "RefrenceDepositRequest");

            migrationBuilder.DropColumn(
                name: "RefrenceDescription",
                table: "RefrenceDepositRequest");
 
        }
    }
}
