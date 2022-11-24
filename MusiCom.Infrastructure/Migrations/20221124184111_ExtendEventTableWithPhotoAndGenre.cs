using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusiCom.Infrastructure.Migrations
{
    public partial class ExtendEventTableWithPhotoAndGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TitlePhoto",
                table: "News",
                newName: "TitleImage");

            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "EventPosts",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "AspNetUsers",
                newName: "Image");

            migrationBuilder.AddColumn<Guid>(
                name: "GenreId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Events",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Events_GenreId",
                table: "Events",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Genres_GenreId",
                table: "Events",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Genres_GenreId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_GenreId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "TitleImage",
                table: "News",
                newName: "TitlePhoto");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "EventPosts",
                newName: "Photo");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "AspNetUsers",
                newName: "Photo");
        }
    }
}
