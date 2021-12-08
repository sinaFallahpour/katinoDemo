using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class edit_advers_createStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "AdverCreatationStatus",
            //    table: "JobAdvertisements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "AdverCreatationStatus",
            //    table: "JobAdvertisements",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }
    }
}
