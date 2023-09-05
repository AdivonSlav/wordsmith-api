using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class EditReportReasons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "report_reasons",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "report_reasons");
        }
    }
}
