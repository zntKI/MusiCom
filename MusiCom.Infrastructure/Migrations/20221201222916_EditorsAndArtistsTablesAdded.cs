using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusiCom.Infrastructure.Migrations
{
    public partial class EditorsAndArtistsTablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ArtistId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EditorId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Editors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HireDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    Salary = table.Column<decimal>(type: "DECIMAL(7,2)", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ArtistId",
                table: "AspNetUsers",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EditorId",
                table: "AspNetUsers",
                column: "EditorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Artists_ArtistId",
                table: "AspNetUsers",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Editors_EditorId",
                table: "AspNetUsers",
                column: "EditorId",
                principalTable: "Editors",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Artists_ArtistId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Editors_EditorId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Editors");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ArtistId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EditorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ArtistId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EditorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");
        }
    }
}
