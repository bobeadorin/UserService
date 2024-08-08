using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a25c4a50-04ba-4bf8-be9d-17818c11844d"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Country", "Currency", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "Username" },
                values: new object[] { new Guid("4d6821c0-6aed-4f80-b8a6-52b3307ce889"), "Str Test", "USA", "USD", "bobeadorin@yahoo.com", "Joe", "Doe", "1f3085b93c4df1d85d28aa5d64efa559c0754bfd68dff0092a8eee16659f917c", "0730733429", "JoeDoeTheFirst" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4d6821c0-6aed-4f80-b8a6-52b3307ce889"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Country", "Currency", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "Username" },
                values: new object[] { new Guid("a25c4a50-04ba-4bf8-be9d-17818c11844d"), "Str Test", "USA", "USD", "bobeadorin@yahoo.com", "Joe", "Doe", "TestPassword123@", "0730733429", "JoeDoeTheFirst" });
        }
    }
}
