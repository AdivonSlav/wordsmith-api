using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class V7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayPalRefundId",
                table: "orders",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundDate",
                table: "orders",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayPalRefundId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "RefundDate",
                table: "orders");
        }
    }
}
