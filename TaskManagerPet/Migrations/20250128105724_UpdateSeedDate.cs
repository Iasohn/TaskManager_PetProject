using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagerPet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "1f30e8d1-9a3c-43d4-a672-e94b78fe1f43", null, "Admin", "ADMIN" },
                    { "92f99f7e-20b7-48b0-b3de-cf5c73bb5bb6", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f30e8d1-9a3c-43d4-a672-e94b78fe1f43");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92f99f7e-20b7-48b0-b3de-cf5c73bb5bb6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "480c2acc-f469-4ab4-8097-e022273f7f1a", null, "User", "USER" },
                    { "b72fed2a-b3d2-4f23-afaa-e741c81c6980", null, "Admin", "ADMIN" }
                });
        }
    }
}
