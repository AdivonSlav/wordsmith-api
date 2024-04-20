using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class V26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notes_ebook_chapters_EBookChapterId",
                table: "notes");

            migrationBuilder.RenameColumn(
                name: "EBookChapterId",
                table: "notes",
                newName: "EBookId");

            migrationBuilder.RenameIndex(
                name: "IX_notes_EBookChapterId",
                table: "notes",
                newName: "IX_notes_EBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_notes_ebooks_EBookId",
                table: "notes",
                column: "EBookId",
                principalTable: "ebooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notes_ebooks_EBookId",
                table: "notes");

            migrationBuilder.RenameColumn(
                name: "EBookId",
                table: "notes",
                newName: "EBookChapterId");

            migrationBuilder.RenameIndex(
                name: "IX_notes_EBookId",
                table: "notes",
                newName: "IX_notes_EBookChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_notes_ebook_chapters_EBookChapterId",
                table: "notes",
                column: "EBookChapterId",
                principalTable: "ebook_chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
