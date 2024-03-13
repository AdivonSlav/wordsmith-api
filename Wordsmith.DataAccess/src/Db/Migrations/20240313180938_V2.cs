using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "orders",
                newName: "ReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_orders_OrderId",
                table: "orders",
                newName: "IX_orders_ReferenceId");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "orders",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "ReferenceId",
                table: "orders",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_orders_ReferenceId",
                table: "orders",
                newName: "IX_orders_OrderId");
        }
    }
}
