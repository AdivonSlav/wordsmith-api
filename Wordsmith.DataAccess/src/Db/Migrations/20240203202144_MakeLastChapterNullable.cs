using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class MakeLastChapterNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_libraries_ebook_chapters_LastChapterId",
                table: "user_libraries");

            migrationBuilder.AlterColumn<int>(
                name: "LastChapterId",
                table: "user_libraries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_user_libraries_ebook_chapters_LastChapterId",
                table: "user_libraries",
                column: "LastChapterId",
                principalTable: "ebook_chapters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_libraries_ebook_chapters_LastChapterId",
                table: "user_libraries");

            migrationBuilder.AlterColumn<int>(
                name: "LastChapterId",
                table: "user_libraries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_user_libraries_ebook_chapters_LastChapterId",
                table: "user_libraries",
                column: "LastChapterId",
                principalTable: "ebook_chapters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
