using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class V15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_comments_EBookChapterId",
                table: "comments",
                column: "EBookChapterId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_ebook_chapters_EBookChapterId",
                table: "comments",
                column: "EBookChapterId",
                principalTable: "ebook_chapters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_ebook_chapters_EBookChapterId",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_EBookChapterId",
                table: "comments");
        }
    }
}
