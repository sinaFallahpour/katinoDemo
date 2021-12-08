using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class AddEmailNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryIds = table.Column<string>(nullable: true),
                    KeyWord = table.Column<string>(nullable: true),
                    TypeOfCooperation = table.Column<int>(nullable: true),
                    EmailNotificationSendTime = table.Column<int>(nullable: false),
                    Cities = table.Column<string>(nullable: true),
                    ShouldSend = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotifications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailNotifications");
        }
    }
}
