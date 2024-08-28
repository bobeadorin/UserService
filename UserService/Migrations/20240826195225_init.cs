using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jwtRefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jwtRefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "serviceLogin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serviceLogin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: false),
                    FollowersNumber = table.Column<int>(type: "int", nullable: false),
                    Posts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostsNumber = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Country", "Currency", "Email", "FirstName", "FollowersNumber", "LastName", "Likes", "Password", "PhoneNumber", "Posts", "PostsNumber", "UserId", "Username" },
                values: new object[] { new Guid("d3e37998-8cbf-4162-ba83-b7f28758b033"), "Str Test", "USA", "USD", "bobeadorin@yahoo.com", "Joe", 0, "Doe", 0, "1f3085b93c4df1d85d28aa5d64efa559c0754bfd68dff0092a8eee16659f917c", "0730733429", null, 0, null, "JoeDoeTheFirst" });

            migrationBuilder.InsertData(
                table: "serviceLogin",
                columns: new[] { "Id", "Password", "Username" },
                values: new object[] { new Guid("b27bcc3a-8ac1-4e59-a9e6-ab1c86bec745"), "a25980c153c7dc3c19c130498f40386e4c7675d6306efe9cdd8845f1b33f0d73", "devService" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jwtRefreshTokens");

            migrationBuilder.DropTable(
                name: "serviceLogin");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
