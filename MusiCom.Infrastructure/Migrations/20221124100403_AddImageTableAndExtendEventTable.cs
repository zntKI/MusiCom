using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusiCom.Infrastructure.Migrations
{
    public partial class AddImageTableAndExtendEventTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitlePhoto",
                table: "News");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "EventPosts");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "News",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GenreId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "EventPosts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Photo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_News_ImageId",
                table: "News",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_GenreId",
                table: "Events",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ImageId",
                table: "Events",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EventPosts_ImageId",
                table: "EventPosts",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_EventPosts_Images_ImageId",
                table: "EventPosts",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Genres_GenreId",
                table: "Events",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Images_ImageId",
                table: "Events",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_News_Images_ImageId",
                table: "News",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventPosts_Images_ImageId",
                table: "EventPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Genres_GenreId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Images_ImageId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_News_Images_ImageId",
                table: "News");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_News_ImageId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_Events_GenreId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ImageId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_EventPosts_ImageId",
                table: "EventPosts");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "EventPosts");

            migrationBuilder.AddColumn<byte[]>(
                name: "TitlePhoto",
                table: "News",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "EventPosts",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
