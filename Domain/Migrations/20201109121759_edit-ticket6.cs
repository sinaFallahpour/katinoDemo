using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editticket6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Answer1",
            //    table: "Tickets");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UserSeenDate",
                table: "Tickets",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnswerDate",
                table: "Tickets",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AdminSeenDate",
                table: "Tickets",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            //migrationBuilder.AddColumn<string>(
            //    name: "Answer",
            //    table: "Tickets",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Tickets");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UserSeenDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnswerDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AdminSeenDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Answer1",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
