using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Source.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostReactsTab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReactes",
                table: "PostReactes");

            migrationBuilder.DropIndex(
                name: "IX_PostReactes_PostId",
                table: "PostReactes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "02139ba2-906d-472d-aa86-5e4692e9163f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e56c1d5-c957-4295-ae7e-41c85f52d5cf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6dbe16d3-82d4-4eaa-a7bb-6b52082278f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf22682f-def0-45fb-9619-f3d872e5f477");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PostReactes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReactes",
                table: "PostReactes",
                columns: new[] { "PostId", "UserId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3617cbfa-1885-4528-8497-f74508ad824a", null, "guest", "Guest" },
                    { "6490cc2e-fd3e-44df-bded-aca8d581b57f", null, "admin", "Admin" },
                    { "941aa08a-1931-40cc-95e0-ddaa8d510bfd", null, "user", "User" },
                    { "fe4ec673-9701-4329-9aab-fe0020304cf3", null, "manage_posts", "Manage_posts" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReactes",
                table: "PostReactes");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3617cbfa-1885-4528-8497-f74508ad824a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6490cc2e-fd3e-44df-bded-aca8d581b57f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "941aa08a-1931-40cc-95e0-ddaa8d510bfd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe4ec673-9701-4329-9aab-fe0020304cf3");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PostReactes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReactes",
                table: "PostReactes",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "02139ba2-906d-472d-aa86-5e4692e9163f", null, "user", "User" },
                    { "5e56c1d5-c957-4295-ae7e-41c85f52d5cf", null, "manage_posts", "Manage_posts" },
                    { "6dbe16d3-82d4-4eaa-a7bb-6b52082278f8", null, "guest", "Guest" },
                    { "cf22682f-def0-45fb-9619-f3d872e5f477", null, "admin", "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostReactes_PostId",
                table: "PostReactes",
                column: "PostId");
        }
    }
}
