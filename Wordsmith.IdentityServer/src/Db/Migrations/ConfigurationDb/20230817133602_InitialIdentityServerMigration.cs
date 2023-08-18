#nullable disable

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wordsmith.IdentityServer.Db.Migrations.ConfigurationDb;

/// <inheritdoc />
public partial class InitialIdentityServerMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ApiResources",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>("tinyint(1)", nullable: false),
                    Name = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>("varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AllowedAccessTokenSigningAlgorithms = table
                        .Column<string>("varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShowInDiscoveryDocument = table.Column<bool>("tinyint(1)", nullable: false),
                    RequireResourceIndicator = table.Column<bool>("tinyint(1)", nullable: false),
                    Created = table.Column<DateTime>("datetime(6)", nullable: false),
                    Updated = table.Column<DateTime>("datetime(6)", nullable: true),
                    LastAccessed = table.Column<DateTime>("datetime(6)", nullable: true),
                    NonEditable = table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ApiResources", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ApiScopes",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>("tinyint(1)", nullable: false),
                    Name = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>("varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Required = table.Column<bool>("tinyint(1)", nullable: false),
                    Emphasize = table.Column<bool>("tinyint(1)", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>("tinyint(1)", nullable: false),
                    Created = table.Column<DateTime>("datetime(6)", nullable: false),
                    Updated = table.Column<DateTime>("datetime(6)", nullable: true),
                    LastAccessed = table.Column<DateTime>("datetime(6)", nullable: true),
                    NonEditable = table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_ApiScopes", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "Clients",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>("tinyint(1)", nullable: false),
                    ClientId = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProtocolType = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequireClientSecret = table.Column<bool>("tinyint(1)", nullable: false),
                    ClientName = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>("varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientUri = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogoUri = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RequireConsent = table.Column<bool>("tinyint(1)", nullable: false),
                    AllowRememberConsent = table.Column<bool>("tinyint(1)", nullable: false),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>("tinyint(1)", nullable: false),
                    RequirePkce = table.Column<bool>("tinyint(1)", nullable: false),
                    AllowPlainTextPkce = table.Column<bool>("tinyint(1)", nullable: false),
                    RequireRequestObject = table.Column<bool>("tinyint(1)", nullable: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>("tinyint(1)", nullable: false),
                    RequireDPoP = table.Column<bool>("tinyint(1)", nullable: false),
                    DPoPValidationMode = table.Column<int>("int", nullable: false),
                    DPoPClockSkew = table.Column<TimeSpan>("time(6)", nullable: false),
                    FrontChannelLogoutUri = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FrontChannelLogoutSessionRequired = table.Column<bool>("tinyint(1)", nullable: false),
                    BackChannelLogoutUri = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BackChannelLogoutSessionRequired = table.Column<bool>("tinyint(1)", nullable: false),
                    AllowOfflineAccess = table.Column<bool>("tinyint(1)", nullable: false),
                    IdentityTokenLifetime = table.Column<int>("int", nullable: false),
                    AllowedIdentityTokenSigningAlgorithms = table
                        .Column<string>("varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccessTokenLifetime = table.Column<int>("int", nullable: false),
                    AuthorizationCodeLifetime = table.Column<int>("int", nullable: false),
                    ConsentLifetime = table.Column<int>("int", nullable: true),
                    AbsoluteRefreshTokenLifetime = table.Column<int>("int", nullable: false),
                    SlidingRefreshTokenLifetime = table.Column<int>("int", nullable: false),
                    RefreshTokenUsage = table.Column<int>("int", nullable: false),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>("tinyint(1)", nullable: false),
                    RefreshTokenExpiration = table.Column<int>("int", nullable: false),
                    AccessTokenType = table.Column<int>("int", nullable: false),
                    EnableLocalLogin = table.Column<bool>("tinyint(1)", nullable: false),
                    IncludeJwtId = table.Column<bool>("tinyint(1)", nullable: false),
                    AlwaysSendClientClaims = table.Column<bool>("tinyint(1)", nullable: false),
                    ClientClaimsPrefix = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PairWiseSubjectSalt = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InitiateLoginUri = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserSsoLifetime = table.Column<int>("int", nullable: true),
                    UserCodeType = table.Column<string>("varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DeviceCodeLifetime = table.Column<int>("int", nullable: false),
                    CibaLifetime = table.Column<int>("int", nullable: true),
                    PollingInterval = table.Column<int>("int", nullable: true),
                    CoordinateLifetimeWithUserSession = table.Column<bool>("tinyint(1)", nullable: true),
                    Created = table.Column<DateTime>("datetime(6)", nullable: false),
                    Updated = table.Column<DateTime>("datetime(6)", nullable: true),
                    LastAccessed = table.Column<DateTime>("datetime(6)", nullable: true),
                    NonEditable = table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Clients", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "IdentityProviders",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Scheme = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enabled = table.Column<bool>("tinyint(1)", nullable: false),
                    Type = table.Column<string>("varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Properties = table.Column<string>("longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>("datetime(6)", nullable: false),
                    Updated = table.Column<DateTime>("datetime(6)", nullable: true),
                    LastAccessed = table.Column<DateTime>("datetime(6)", nullable: true),
                    NonEditable = table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_IdentityProviders", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "IdentityResources",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Enabled = table.Column<bool>("tinyint(1)", nullable: false),
                    Name = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>("varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>("varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Required = table.Column<bool>("tinyint(1)", nullable: false),
                    Emphasize = table.Column<bool>("tinyint(1)", nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>("tinyint(1)", nullable: false),
                    Created = table.Column<DateTime>("datetime(6)", nullable: false),
                    Updated = table.Column<DateTime>("datetime(6)", nullable: true),
                    NonEditable = table.Column<bool>("tinyint(1)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_IdentityResources", x => x.Id); })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ApiResourceClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApiResourceId = table.Column<int>("int", nullable: false),
                    Type = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResourceClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiResourceClaims_ApiResources_ApiResourceId",
                        x => x.ApiResourceId,
                        "ApiResources",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ApiResourceProperties",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApiResourceId = table.Column<int>("int", nullable: false),
                    Key = table.Column<string>("varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResourceProperties", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiResourceProperties_ApiResources_ApiResourceId",
                        x => x.ApiResourceId,
                        "ApiResources",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ApiResourceScopes",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Scope = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ApiResourceId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiResourceScopes_ApiResources_ApiResourceId",
                        x => x.ApiResourceId,
                        "ApiResources",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ApiResourceSecrets",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApiResourceId = table.Column<int>("int", nullable: false),
                    Description = table.Column<string>("varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>("varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Expiration = table.Column<DateTime>("datetime(6)", nullable: true),
                    Type = table.Column<string>("varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>("datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResourceSecrets", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiResourceSecrets_ApiResources_ApiResourceId",
                        x => x.ApiResourceId,
                        "ApiResources",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ApiScopeClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ScopeId = table.Column<int>("int", nullable: false),
                    Type = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiScopeClaims_ApiScopes_ScopeId",
                        x => x.ScopeId,
                        "ApiScopes",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ApiScopeProperties",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ScopeId = table.Column<int>("int", nullable: false),
                    Key = table.Column<string>("varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopeProperties", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiScopeProperties_ApiScopes_ScopeId",
                        x => x.ScopeId,
                        "ApiScopes",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ClientClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>("varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>("varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientClaims_Clients_ClientId",
                        x => x.ClientId,
                        "Clients",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ClientCorsOrigins",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Origin = table.Column<string>("varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientCorsOrigins_Clients_ClientId",
                        x => x.ClientId,
                        "Clients",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ClientGrantTypes",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GrantType = table.Column<string>("varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientGrantTypes_Clients_ClientId",
                        x => x.ClientId,
                        "Clients",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ClientIdPRestrictions",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Provider = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientIdPRestrictions_Clients_ClientId",
                        x => x.ClientId,
                        "Clients",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ClientPostLogoutRedirectUris",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PostLogoutRedirectUri = table.Column<string>("varchar(400)", maxLength: 400, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientPostLogoutRedirectUris_Clients_ClientId",
                        x => x.ClientId,
                        "Clients",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ClientProperties",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>("int", nullable: false),
                    Key = table.Column<string>("varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProperties", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientProperties_Clients_ClientId",
                        x => x.ClientId,
                        "Clients",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ClientRedirectUris",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RedirectUri = table.Column<string>("varchar(400)", maxLength: 400, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientRedirectUris_Clients_ClientId",
                        x => x.ClientId,
                        "Clients",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ClientScopes",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Scope = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClientId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientScopes", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientScopes_Clients_ClientId",
                        x => x.ClientId,
                        "Clients",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "ClientSecrets",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>("int", nullable: false),
                    Description = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>("varchar(4000)", maxLength: 4000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Expiration = table.Column<DateTime>("datetime(6)", nullable: true),
                    Type = table.Column<string>("varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTime>("datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientSecrets_Clients_ClientId",
                        x => x.ClientId,
                        "Clients",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "IdentityResourceClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityResourceId = table.Column<int>("int", nullable: false),
                    Type = table.Column<string>("varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityResourceClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_IdentityResourceClaims_IdentityResources_IdentityResourceId",
                        x => x.IdentityResourceId,
                        "IdentityResources",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
                "IdentityResourceProperties",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdentityResourceId = table.Column<int>("int", nullable: false),
                    Key = table.Column<string>("varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<string>("varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityResourceProperties", x => x.Id);
                    table.ForeignKey(
                        "FK_IdentityResourceProperties_IdentityResources_IdentityResourc~",
                        x => x.IdentityResourceId,
                        "IdentityResources",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            "IX_ApiResourceClaims_ApiResourceId_Type",
            "ApiResourceClaims",
            new[] { "ApiResourceId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceProperties_ApiResourceId_Key",
            "ApiResourceProperties",
            new[] { "ApiResourceId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResources_Name",
            "ApiResources",
            "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceScopes_ApiResourceId_Scope",
            "ApiResourceScopes",
            new[] { "ApiResourceId", "Scope" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiResourceSecrets_ApiResourceId",
            "ApiResourceSecrets",
            "ApiResourceId");

        migrationBuilder.CreateIndex(
            "IX_ApiScopeClaims_ScopeId_Type",
            "ApiScopeClaims",
            new[] { "ScopeId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiScopeProperties_ScopeId_Key",
            "ApiScopeProperties",
            new[] { "ScopeId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ApiScopes_Name",
            "ApiScopes",
            "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientClaims_ClientId_Type_Value",
            "ClientClaims",
            new[] { "ClientId", "Type", "Value" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientCorsOrigins_ClientId_Origin",
            "ClientCorsOrigins",
            new[] { "ClientId", "Origin" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientGrantTypes_ClientId_GrantType",
            "ClientGrantTypes",
            new[] { "ClientId", "GrantType" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientIdPRestrictions_ClientId_Provider",
            "ClientIdPRestrictions",
            new[] { "ClientId", "Provider" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientPostLogoutRedirectUris_ClientId_PostLogoutRedirectUri",
            "ClientPostLogoutRedirectUris",
            new[] { "ClientId", "PostLogoutRedirectUri" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientProperties_ClientId_Key",
            "ClientProperties",
            new[] { "ClientId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientRedirectUris_ClientId_RedirectUri",
            "ClientRedirectUris",
            new[] { "ClientId", "RedirectUri" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_Clients_ClientId",
            "Clients",
            "ClientId",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientScopes_ClientId_Scope",
            "ClientScopes",
            new[] { "ClientId", "Scope" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_ClientSecrets_ClientId",
            "ClientSecrets",
            "ClientId");

        migrationBuilder.CreateIndex(
            "IX_IdentityProviders_Scheme",
            "IdentityProviders",
            "Scheme",
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResourceClaims_IdentityResourceId_Type",
            "IdentityResourceClaims",
            new[] { "IdentityResourceId", "Type" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResourceProperties_IdentityResourceId_Key",
            "IdentityResourceProperties",
            new[] { "IdentityResourceId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            "IX_IdentityResources_Name",
            "IdentityResources",
            "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "ApiResourceClaims");

        migrationBuilder.DropTable(
            "ApiResourceProperties");

        migrationBuilder.DropTable(
            "ApiResourceScopes");

        migrationBuilder.DropTable(
            "ApiResourceSecrets");

        migrationBuilder.DropTable(
            "ApiScopeClaims");

        migrationBuilder.DropTable(
            "ApiScopeProperties");

        migrationBuilder.DropTable(
            "ClientClaims");

        migrationBuilder.DropTable(
            "ClientCorsOrigins");

        migrationBuilder.DropTable(
            "ClientGrantTypes");

        migrationBuilder.DropTable(
            "ClientIdPRestrictions");

        migrationBuilder.DropTable(
            "ClientPostLogoutRedirectUris");

        migrationBuilder.DropTable(
            "ClientProperties");

        migrationBuilder.DropTable(
            "ClientRedirectUris");

        migrationBuilder.DropTable(
            "ClientScopes");

        migrationBuilder.DropTable(
            "ClientSecrets");

        migrationBuilder.DropTable(
            "IdentityProviders");

        migrationBuilder.DropTable(
            "IdentityResourceClaims");

        migrationBuilder.DropTable(
            "IdentityResourceProperties");

        migrationBuilder.DropTable(
            "ApiResources");

        migrationBuilder.DropTable(
            "ApiScopes");

        migrationBuilder.DropTable(
            "Clients");

        migrationBuilder.DropTable(
            "IdentityResources");
    }
}