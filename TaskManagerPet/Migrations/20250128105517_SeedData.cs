using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagerPet.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39000246-7ed8-4378-b328-16f5310a286a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb5622a3-c87d-49cd-b728-ea31aaf7bf3e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "480c2acc-f469-4ab4-8097-e022273f7f1a", null, "User", "USER" },
                    { "b72fed2a-b3d2-4f23-afaa-e741c81c6980", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "480c2acc-f469-4ab4-8097-e022273f7f1a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b72fed2a-b3d2-4f23-afaa-e741c81c6980");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39000246-7ed8-4378-b328-16f5310a286a", null, "User", "USER" },
                    { "cb5622a3-c87d-49cd-b728-ea31aaf7bf3e", null, "Admin", "ADMIN" }
                });
        }
    }
}
