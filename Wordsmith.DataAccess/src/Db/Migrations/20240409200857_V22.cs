using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class V22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "RatingAverage",
                table: "ebooks",
                type: "double",
                nullable: true,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "RatingAverage",
                table: "ebooks",
                type: "double",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true,
                oldDefaultValue: 0.0);
        }
    }
}
