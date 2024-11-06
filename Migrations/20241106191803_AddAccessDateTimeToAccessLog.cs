using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace urlshorter.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessDateTimeToAccessLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AccessDateTime",
                table: "AccessLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_AccessLogs_UrlId",
                table: "AccessLogs",
                column: "UrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessLogs_Urls_UrlId",
                table: "AccessLogs",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessLogs_Urls_UrlId",
                table: "AccessLogs");

            migrationBuilder.DropIndex(
                name: "IX_AccessLogs_UrlId",
                table: "AccessLogs");

            migrationBuilder.DropColumn(
                name: "AccessDateTime",
                table: "AccessLogs");
        }
    }
}
