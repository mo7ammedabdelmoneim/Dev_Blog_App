using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Source.Migrations
{
    /// <inheritdoc />
    public partial class AddPostReactsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52f08a99-ccc1-456b-8b51-54b9d27f4a42");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b25a472-c523-43f2-b856-190f0f5b66cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8e45e0cb-ec65-492c-9fe3-cefaeb0b13de");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d950a69a-c62a-422b-85b8-33782211242e");

            migrationBuilder.CreateTable(
                name: "PostReactes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostReactes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostReactes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostReactes_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "227ebdb2-8579-48c1-a380-d458c87411c5", null, "manage_posts", "Manage_posts" },
                    { "ce4b08e3-c414-4213-b9e5-593d332b5598", null, "guest", "Guest" },
                    { "f3b26c30-079e-4034-8f96-c2cd13001cba", null, "user", "User" },
                    { "fa67f0bf-1dee-4253-8dc0-05b82922cd8c", null, "admin", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostReactes_PostId",
                table: "PostReactes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostReactes_UserId",
                table: "PostReactes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostReactes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "227ebdb2-8579-48c1-a380-d458c87411c5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce4b08e3-c414-4213-b9e5-593d332b5598");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3b26c30-079e-4034-8f96-c2cd13001cba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fa67f0bf-1dee-4253-8dc0-05b82922cd8c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52f08a99-ccc1-456b-8b51-54b9d27f4a42", null, "user", "User" },
                    { "5b25a472-c523-43f2-b856-190f0f5b66cc", null, "manage_posts", "Manage_posts" },
                    { "8e45e0cb-ec65-492c-9fe3-cefaeb0b13de", null, "admin", "Admin" },
                    { "d950a69a-c62a-422b-85b8-33782211242e", null, "guest", "Guest" }
                });
        }
    }
}
