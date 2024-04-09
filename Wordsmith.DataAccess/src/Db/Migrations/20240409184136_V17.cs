using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class V17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "report_reasons");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "report_reasons",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "report_reasons");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "report_reasons",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
