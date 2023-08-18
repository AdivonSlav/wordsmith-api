#nullable disable

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wordsmith.IdentityServer.Db.Migrations.PersistedGrantDb;

/// <inheritdoc />
public partial class InitialIdentityServerMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "DeviceCodes",
                table => new
                {
                    UserCode = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceCode = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectId = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionId = table.Column<string>("varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>("datetime(6)", nullable: false),
                    Expiration = table.Column<DateTime>("datetime(6)", nullable: false),
                    Data = table.Column<string>("longtext", maxLength: 50000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table => { table.PrimaryKey("PK_DeviceCodes", x => x.UserCode); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "Keys",
                table => new
                {
                    Id = table.Column<string>("varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Version = table.Column<int>("int", nullable: false),
                    Created = table.Column<DateTime>("datetime(6)", nullable: false),
                    Use = table.Column<string>("varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Algorithm = table.Column<string>("varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsX509Certificate = table.Column<bool>("tinyint(1)", nullable: false),
                    DataProtected = table.Column<bool>("tinyint(1)", nullable: false),
                    Data = table.Column<string>("longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table => { table.PrimaryKey("PK_Keys", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "PersistedGrants",
                table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>("varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectId = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionId = table.Column<string>("varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationTime = table.Column<DateTime>("datetime(6)", nullable: false),
                    Expiration = table.Column<DateTime>("datetime(6)", nullable: true),
                    ConsumedTime = table.Column<DateTime>("datetime(6)", nullable: true),
                    Data = table.Column<string>("longtext", maxLength: 50000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table => { table.PrimaryKey("PK_PersistedGrants", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ServerSideSessions",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>("varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Scheme = table.Column<string>("varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubjectId = table.Column<string>("varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SessionId = table.Column<string>("varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>("varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>("datetime(6)", nullable: false),
                    Renewed = table.Column<DateTime>("datetime(6)", nullable: false),
                    Expires = table.Column<DateTime>("datetime(6)", nullable: true),
                    Data = table.Column<string>("longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table => { table.PrimaryKey("PK_ServerSideSessions", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            "IX_DeviceCodes_DeviceCode",
            "DeviceCodes",
            "DeviceCode",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_DeviceCodes_Expiration",
            "DeviceCodes",
            "Expiration");

        migrationBuilder.CreateIndex(
            "IX_Keys_Use",
            "Keys",
            "Use");

        migrationBuilder.CreateIndex(
            "IX_PersistedGrants_ConsumedTime",
            "PersistedGrants",
            "ConsumedTime");

        migrationBuilder.CreateIndex(
            "IX_PersistedGrants_Expiration",
            "PersistedGrants",
            "Expiration");

        migrationBuilder.CreateIndex(
            "IX_PersistedGrants_Key",
            "PersistedGrants",
            "Key",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_PersistedGrants_SubjectId_ClientId_Type",
            "PersistedGrants",
            new[] { "SubjectId", "ClientId", "Type" });

        migrationBuilder.CreateIndex(
            "IX_PersistedGrants_SubjectId_SessionId_Type",
            "PersistedGrants",
            new[] { "SubjectId", "SessionId", "Type" });

        migrationBuilder.CreateIndex(
            "IX_ServerSideSessions_DisplayName",
            "ServerSideSessions",
            "DisplayName");

        migrationBuilder.CreateIndex(
            "IX_ServerSideSessions_Expires",
            "ServerSideSessions",
            "Expires");

        migrationBuilder.CreateIndex(
            "IX_ServerSideSessions_Key",
            "ServerSideSessions",
            "Key",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ServerSideSessions_SessionId",
            "ServerSideSessions",
            "SessionId");

        migrationBuilder.CreateIndex(
            "IX_ServerSideSessions_SubjectId",
            "ServerSideSessions",
            "SubjectId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "DeviceCodes");

        migrationBuilder.DropTable(
            "Keys");

        migrationBuilder.DropTable(
            "PersistedGrants");

        migrationBuilder.DropTable(
            "ServerSideSessions");
    }
}