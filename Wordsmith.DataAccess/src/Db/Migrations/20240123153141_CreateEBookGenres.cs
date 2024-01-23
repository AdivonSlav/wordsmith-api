using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateEBookGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ebook_genres_GenreId",
                table: "ebook");

            migrationBuilder.DropIndex(
                name: "IX_ebook_GenreId",
                table: "ebook");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "ebook");

            migrationBuilder.AddColumn<string>(
                name: "Genres",
                table: "ebook",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ebook_genres",
                columns: table => new
                {
                    EBookId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ebook_genres", x => new { x.EBookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_ebook_genres_ebook_EBookId",
                        column: x => x.EBookId,
                        principalTable: "ebook",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ebook_genres_genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_genres_GenreId",
                table: "ebook_genres",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ebook_genres");

            migrationBuilder.DropColumn(
                name: "Genres",
                table: "ebook");

            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "ebook",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ebook_GenreId",
                table: "ebook",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_ebook_genres_GenreId",
                table: "ebook",
                column: "GenreId",
                principalTable: "genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
