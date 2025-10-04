using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Source.Migrations
{
    /// <inheritdoc />
    public partial class devBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_PostId",
                table: "Tags");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1211c404-e703-4c4e-aac9-92b81245ade0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "73ccbe9b-8a64-440b-93ba-e23f706a2ee6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87b3d85b-45a7-4539-8a88-aee6697fadfa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e679e171-2927-4421-8096-4b8f70d9112c");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Tags");

            migrationBuilder.AddColumn<int>(
                name: "Reacts",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PostTag",
                columns: table => new
                {
                    PostsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTag", x => new { x.PostsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PostTag_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PostTag_TagsId",
                table: "PostTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostTag");

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

            migrationBuilder.DropColumn(
                name: "Reacts",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Posts");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1211c404-e703-4c4e-aac9-92b81245ade0", null, "guest", "Guest" },
                    { "73ccbe9b-8a64-440b-93ba-e23f706a2ee6", null, "user", "User" },
                    { "87b3d85b-45a7-4539-8a88-aee6697fadfa", null, "admin", "Admin" },
                    { "e679e171-2927-4421-8096-4b8f70d9112c", null, "manage_posts", "Manage_posts" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_PostId",
                table: "Tags",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Posts_PostId",
                table: "Tags",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");
        }
    }
}
