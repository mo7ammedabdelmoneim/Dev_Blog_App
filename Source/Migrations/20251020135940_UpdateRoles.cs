using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Source.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "007df574-2fbb-4da7-a75e-69cae14254fc", null, "admin", "ADMIN" },
                    { "08ee5cb8-3424-45ec-a9db-40af5e2d4153", null, "manage_posts", "MANGE_POSTS" },
                    { "e15d6886-8f02-4a9b-9604-8961d8b61532", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "007df574-2fbb-4da7-a75e-69cae14254fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "08ee5cb8-3424-45ec-a9db-40af5e2d4153");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e15d6886-8f02-4a9b-9604-8961d8b61532");

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
    }
}
