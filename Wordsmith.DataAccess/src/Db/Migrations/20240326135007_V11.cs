using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class V11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_comments_EBookId",
                table: "comments",
                column: "EBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_ebooks_EBookId",
                table: "comments",
                column: "EBookId",
                principalTable: "ebooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_ebooks_EBookId",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_EBookId",
                table: "comments");
        }
    }
}
