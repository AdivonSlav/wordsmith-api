using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class V25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharBegin",
                table: "notes");

            migrationBuilder.DropColumn(
                name: "CharEnd",
                table: "notes");

            migrationBuilder.DropColumn(
                name: "Page",
                table: "notes");

            migrationBuilder.AddColumn<string>(
                name: "Cfi",
                table: "notes",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cfi",
                table: "notes");

            migrationBuilder.AddColumn<int>(
                name: "CharBegin",
                table: "notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CharEnd",
                table: "notes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Page",
                table: "notes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
