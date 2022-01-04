using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace StoryService.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Tale = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stories_ExternalId",
                table: "Stories",
                column: "ExternalId",
                unique: true);

            migrationBuilder.Sql(@"
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97050103987358, 12.368329953256506, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')

                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97054410045045, 12.367393211298968, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')

                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 0, '', '', 43.97014910010278, 12.369785099203499, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')


                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.98669717617231, 12.655767172305104, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')

                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.9918338168829, 12.658979397437323, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')

                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')
                INSERT INTO Stories (ExternalId, [Type], Title, Tale, Latitude, Longitude, PublicationDate, UserId) VALUES (NEWID(), 1, '', '', 43.991347332900055, 12.654960944829272, GETDATE(), '7bb2891d-9645-4b2c-b02f-99fef892a699')

                UPDATE Stories SET
	                Title='My Ghost Story'
                WHERE [Type]=0

                UPDATE Stories SET
	                Title='My Alien Story'
                WHERE [Type]=1

                UPDATE Stories SET
	                Tale='Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Turpis nunc eget lorem dolor sed viverra. Ante in nibh mauris cursus. Vel elit scelerisque mauris pellentesque pulvinar pellentesque habitant. Justo nec ultrices dui sapien. Odio aenean sed adipiscing diam donec. Sit amet nisl purus in mollis nunc sed id semper. Sed viverra tellus in hac habitasse platea dictumst vestibulum. Lorem ipsum dolor sit amet. Natoque penatibus et magnis dis parturient montes nascetur ridiculus mus. Eu sem integer vitae justo eget magna. Morbi tristique senectus et netus et malesuada fames. Blandit cursus risus at ultrices. Lacus vestibulum sed arcu non odio euismod lacinia at. Vitae congue mauris rhoncus aenean vel. Mi proin sed libero enim sed faucibus turpis.'
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stories");
        }
    }
}