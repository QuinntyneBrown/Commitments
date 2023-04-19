using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoteService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NoteInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Note");

            migrationBuilder.CreateTable(
                name: "Notes",
                schema: "Note",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "Note",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "NoteTag",
                schema: "Note",
                columns: table => new
                {
                    NotesNoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagsTagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTag", x => new { x.NotesNoteId, x.TagsTagId });
                    table.ForeignKey(
                        name: "FK_NoteTag_Notes_NotesNoteId",
                        column: x => x.NotesNoteId,
                        principalSchema: "Note",
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteTag_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalSchema: "Note",
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteTag_TagsTagId",
                schema: "Note",
                table: "NoteTag",
                column: "TagsTagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteTag",
                schema: "Note");

            migrationBuilder.DropTable(
                name: "Notes",
                schema: "Note");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "Note");
        }
    }
}