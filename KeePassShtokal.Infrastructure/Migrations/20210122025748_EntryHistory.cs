using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KeePassShtokal.Infrastructure.Migrations
{
    public partial class EntryHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "PasswordE",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "WebAddress",
                table: "Entries");

            migrationBuilder.AddColumn<int>(
                name: "CurrentEntryStateId",
                table: "Entries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EntryStates",
                columns: table => new
                {
                    EntryStateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryStates", x => x.EntryStateId);
                });

            migrationBuilder.CreateTable(
                name: "EntryActions",
                columns: table => new
                {
                    ActionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryStateId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EntryId = table.Column<int>(type: "int", nullable: false),
                    IsRestorable = table.Column<bool>(type: "bit", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryActions", x => x.ActionId);
                    table.ForeignKey(
                        name: "FK_EntryActions_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntryActions_EntryStates_EntryStateId",
                        column: x => x.EntryStateId,
                        principalTable: "EntryStates",
                        principalColumn: "EntryStateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntryActions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_CurrentEntryStateId",
                table: "Entries",
                column: "CurrentEntryStateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntryActions_EntryId",
                table: "EntryActions",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryActions_EntryStateId",
                table: "EntryActions",
                column: "EntryStateId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryActions_UserId",
                table: "EntryActions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_EntryStates_CurrentEntryStateId",
                table: "Entries",
                column: "CurrentEntryStateId",
                principalTable: "EntryStates",
                principalColumn: "EntryStateId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_EntryStates_CurrentEntryStateId",
                table: "Entries");

            migrationBuilder.DropTable(
                name: "EntryActions");

            migrationBuilder.DropTable(
                name: "EntryStates");

            migrationBuilder.DropIndex(
                name: "IX_Entries_CurrentEntryStateId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "CurrentEntryStateId",
                table: "Entries");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordE",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WebAddress",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
