using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class scvfgh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "ImageAddress",
            //    table: "JobAdvertisements",
            //    nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastSeen",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<double>(
                name: "RefrencePercent",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "RefrenceDepositRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(nullable: false),
                    RefrenceId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    RefrenceTransationStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefrenceDepositRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefrenceDepositRequest_AspNetUsers_RefrenceId",
                        column: x => x.RefrenceId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefrenceDepositRequest_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefrenceTransations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(nullable: false),
                    RefrenceId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    DepositStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefrenceTransations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefrenceTransations_AspNetUsers_RefrenceId",
                        column: x => x.RefrenceId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefrenceTransations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefrenceDepositRequest_RefrenceId",
                table: "RefrenceDepositRequest",
                column: "RefrenceId");

            migrationBuilder.CreateIndex(
                name: "IX_RefrenceDepositRequest_UserId",
                table: "RefrenceDepositRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefrenceTransations_RefrenceId",
                table: "RefrenceTransations",
                column: "RefrenceId");

            migrationBuilder.CreateIndex(
                name: "IX_RefrenceTransations_UserId",
                table: "RefrenceTransations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefrenceDepositRequest");

            migrationBuilder.DropTable(
                name: "RefrenceTransations");

            //migrationBuilder.DropColumn(
            //    name: "ImageAddress",
            //    table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "RefrencePercent",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastSeen",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
