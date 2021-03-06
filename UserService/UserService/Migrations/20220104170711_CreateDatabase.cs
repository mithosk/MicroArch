using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace UserService.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    AccessKey = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stories_ExternalId",
                table: "Stories",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stories_UserId",
                table: "Stories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccessKey",
                table: "Users",
                column: "AccessKey",
                unique: true,
                filter: "[AccessKey] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalId",
                table: "Users",
                column: "ExternalId",
                unique: true);

            migrationBuilder.Sql(@"
                INSERT INTO Users (ExternalId, Email, PasswordHash, [Name], Surname, RegistrationDate) VALUES ('7bb2891d-9645-4b2c-b02f-99fef892a699', 'mithosk85@gmail.com',         '918B9FD83DE4910112EAEC1E6AFB4FE5DDEBF5F26872B814895F1E8032FC5EBB6D42FA9708852C4A0D58874B32F314EE91BDB7624F83632A848916D84B384EF4', 'Luca',    'Nicolini', GETUTCDATE())
                INSERT INTO Users (ExternalId, Email, PasswordHash, [Name], Surname, RegistrationDate) VALUES ('b2524fde-43c4-4d6d-ad3f-559adf30c1d9', 'jesty.ricci@gmail.com',       'A23EA03548E96FCA49043A0C5A1D4706642A632857FE5626CDA67F3CF223B56F49C9B8D6535823FC8EC60F341887DDB31E264CEA0AF0ED762B068BBDA2C13A85', 'Jessica', 'Ricci',    GETUTCDATE())
                INSERT INTO Users (ExternalId, Email, PasswordHash, [Name], Surname, RegistrationDate) VALUES ('446ae8cf-0610-4358-947f-35749f953b7e', 'andrea.nicolini93@gmail.com', '46BDE744CB9E30B175D31EA49F31AE9375E802B4056265D78C7B9F6DA7EB72068F6736B30C27C0F0BC5B3FC7413A8D8FF11BCE28B47ECF0D282FCF1AD6527972', 'Andrea',  'Nicolini', GETUTCDATE())
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}