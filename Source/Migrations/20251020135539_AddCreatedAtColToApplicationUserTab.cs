using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Source.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtColToApplicationUserTab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32836804-375a-45e1-b1eb-485771d4dc35");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8c193367-2b54-4548-856c-12b057a2c108");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e279f3bf-c205-4e9b-a08a-846d4301bfcc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e307bc34-45dc-4d79-ae9a-e19905635e8a");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "69fb3754-1485-4de0-aed0-4ce2409ab0d3", null, "admin", "ADMIN" },
                    { "6ae80efe-ddf4-48b5-8ea0-c2ac641801a3", null, "guest", "GUEST" },
                    { "b66644a2-0fad-4857-9c26-1cf49819c9ed", null, "user", "USER" },
                    { "ef257a8e-c9b4-42bf-a83f-aa000666d859", null, "manage_posts", "MANGE_POSTS" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69fb3754-1485-4de0-aed0-4ce2409ab0d3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ae80efe-ddf4-48b5-8ea0-c2ac641801a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b66644a2-0fad-4857-9c26-1cf49819c9ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ef257a8e-c9b4-42bf-a83f-aa000666d859");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32836804-375a-45e1-b1eb-485771d4dc35", null, "admin", "Admin" },
                    { "8c193367-2b54-4548-856c-12b057a2c108", null, "manage_posts", "Manage_posts" },
                    { "e307bc34-45dc-4d79-ae9a-e19905635e8a", null, "user", "User" }
                });
        }
    }
}
