using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Source.Migrations
{
    /// <inheritdoc />
    public partial class RemoveReactsColFromPostTab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "Reacts",
                table: "Posts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32836804-375a-45e1-b1eb-485771d4dc35", null, "admin", "Admin" },
                    { "8c193367-2b54-4548-856c-12b057a2c108", null, "manage_posts", "Manage_posts" },
                    { "e279f3bf-c205-4e9b-a08a-846d4301bfcc", null, "guest", "Guest" },
                    { "e307bc34-45dc-4d79-ae9a-e19905635e8a", null, "user", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Reacts",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
