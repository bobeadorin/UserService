using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class JwtTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4d6821c0-6aed-4f80-b8a6-52b3307ce889"));

            migrationBuilder.CreateTable(
                name: "jwtRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jwtRefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_jwtRefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Country", "Currency", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "Username" },
                values: new object[] { new Guid("bf04748a-ef7a-4faa-92de-80a7f0af2f43"), "Str Test", "USA", "USD", "bobeadorin@yahoo.com", "Joe", "Doe", "1f3085b93c4df1d85d28aa5d64efa559c0754bfd68dff0092a8eee16659f917c", "0730733429", "JoeDoeTheFirst" });

            migrationBuilder.CreateIndex(
                name: "IX_jwtRefreshTokens_UserId",
                table: "jwtRefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jwtRefreshTokens");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bf04748a-ef7a-4faa-92de-80a7f0af2f43"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Country", "Currency", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "Username" },
                values: new object[] { new Guid("4d6821c0-6aed-4f80-b8a6-52b3307ce889"), "Str Test", "USA", "USD", "bobeadorin@yahoo.com", "Joe", "Doe", "1f3085b93c4df1d85d28aa5d64efa559c0754bfd68dff0092a8eee16659f917c", "0730733429", "JoeDoeTheFirst" });
        }
    }
}
