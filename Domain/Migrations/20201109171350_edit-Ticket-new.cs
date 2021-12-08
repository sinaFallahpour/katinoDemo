using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class editTicketnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AdminSeenDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "AnswerFile",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "File",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsAdminSeen",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsFromAdmin",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsUsrerSeen",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UserSeenDate",
                table: "Tickets");

            migrationBuilder.AddColumn<bool>(
                name: "IsReciverSeen",
                table: "Tickets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSenderSeen",
                table: "Tickets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverFile",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiverSeenDate",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderFile",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Tickets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SenderSeenDate",
                table: "Tickets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ReceiverId",
                table: "Tickets",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SenderId",
                table: "Tickets",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_ReceiverId",
                table: "Tickets",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_SenderId",
                table: "Tickets",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_ReceiverId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_SenderId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ReceiverId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SenderId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsReciverSeen",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsSenderSeen",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ReceiverFile",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ReceiverSeenDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SenderFile",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SenderSeenDate",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "AdminSeenDate",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnswerFile",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "File",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdminSeen",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFromAdmin",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsrerSeen",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UserSeenDate",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
