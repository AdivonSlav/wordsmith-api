using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "ebook",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ebook_AuthorId",
                table: "ebook",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ebook_users_AuthorId",
                table: "ebook",
                column: "AuthorId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ebook_users_AuthorId",
                table: "ebook");

            migrationBuilder.DropIndex(
                name: "IX_ebook_AuthorId",
                table: "ebook");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "ebook");
        }
    }
}
