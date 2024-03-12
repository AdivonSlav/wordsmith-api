using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genres", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Path = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Format = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maturity_ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShortName = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_maturity_ratings", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "report_reasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Reason = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Subject = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_reasons", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    About = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RegistrationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Role = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfileImageId = table.Column<int>(type: "int", nullable: true),
                    PayPalEmail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_images_ProfileImageId",
                        column: x => x.ProfileImageId,
                        principalTable: "images",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "app_reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsClosed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_app_reports_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "author_follows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorUserId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_author_follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_author_follows_users_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_author_follows_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsShown = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EBookChapterId = table.Column<int>(type: "int", nullable: true),
                    EBookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comments_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ebooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(800)", maxLength: 800, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RatingAverage = table.Column<double>(type: "double", nullable: true),
                    PublishedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    ChapterCount = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsHidden = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HiddenDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    CoverArtId = table.Column<int>(type: "int", nullable: false),
                    Genres = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaturityRatingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ebooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ebooks_images_CoverArtId",
                        column: x => x.CoverArtId,
                        principalTable: "images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ebooks_maturity_ratings_MaturityRatingId",
                        column: x => x.MaturityRatingId,
                        principalTable: "maturity_ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ebooks_users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "report_details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsClosed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReportReasonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_report_details_report_reasons_ReportReasonId",
                        column: x => x.ReportReasonId,
                        principalTable: "report_reasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_report_details_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_bans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BannedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AdminId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_bans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_bans_users_AdminId",
                        column: x => x.AdminId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_bans_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_library_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_library_categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_library_categories_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ebook_chapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChapterName = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChapterNumber = table.Column<int>(type: "int", nullable: false),
                    EBookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ebook_chapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ebook_chapters_ebooks_EBookId",
                        column: x => x.EBookId,
                        principalTable: "ebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ebook_genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EBookId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ebook_genres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ebook_genres_ebooks_EBookId",
                        column: x => x.EBookId,
                        principalTable: "ebooks",
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

            migrationBuilder.CreateTable(
                name: "ebook_promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PromotionStart = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PromotionEnd = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PromotionLength = table.Column<int>(type: "int", nullable: false),
                    IsInProgress = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EBookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ebook_promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ebook_promotions_ebooks_EBookId",
                        column: x => x.EBookId,
                        principalTable: "ebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ebook_promotions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ebook_ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    RatingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EBookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ebook_ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ebook_ratings_ebooks_EBookId",
                        column: x => x.EBookId,
                        principalTable: "ebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ebook_ratings_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "favorite_ebooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EBookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_ebooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_favorite_ebooks_ebooks_EBookId",
                        column: x => x.EBookId,
                        principalTable: "ebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_favorite_ebooks_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PayerId = table.Column<int>(type: "int", nullable: true),
                    PayerPayPalEmail = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PayeeId = table.Column<int>(type: "int", nullable: true),
                    PayeePayPalEmail = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EBookId = table.Column<int>(type: "int", nullable: true),
                    EBookTitle = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaymentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orders_ebooks_EBookId",
                        column: x => x.EBookId,
                        principalTable: "ebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_users_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_users_PayerId",
                        column: x => x.PayerId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ebook_reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EBookId = table.Column<int>(type: "int", nullable: false),
                    ReportDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ebook_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ebook_reports_ebooks_EBookId",
                        column: x => x.EBookId,
                        principalTable: "ebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ebook_reports_report_details_ReportDetailsId",
                        column: x => x.ReportDetailsId,
                        principalTable: "report_details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReportedUserId = table.Column<int>(type: "int", nullable: false),
                    ReportDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_reports_report_details_ReportDetailsId",
                        column: x => x.ReportDetailsId,
                        principalTable: "report_details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_reports_users_ReportedUserId",
                        column: x => x.ReportedUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Page = table.Column<int>(type: "int", nullable: false),
                    CharBegin = table.Column<int>(type: "int", nullable: false),
                    CharEnd = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "varchar(400)", maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateAdded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EBookChapterId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notes_ebook_chapters_EBookChapterId",
                        column: x => x.EBookChapterId,
                        principalTable: "ebook_chapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notes_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_libraries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EBookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SyncDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReadProgress = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastChapterId = table.Column<int>(type: "int", nullable: true),
                    LastPage = table.Column<int>(type: "int", nullable: false),
                    UserLibraryCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_libraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_libraries_ebook_chapters_LastChapterId",
                        column: x => x.LastChapterId,
                        principalTable: "ebook_chapters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_user_libraries_ebooks_EBookId",
                        column: x => x.EBookId,
                        principalTable: "ebooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_libraries_user_library_categories_UserLibraryCategoryId",
                        column: x => x.UserLibraryCategoryId,
                        principalTable: "user_library_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_user_libraries_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fiction" },
                    { 2, "Mystery" },
                    { 3, "Thriller" },
                    { 4, "Science Fiction" },
                    { 5, "Fantasy" },
                    { 6, "Romance" },
                    { 7, "Historical Fiction" },
                    { 8, "Horror" },
                    { 9, "Adventure" },
                    { 10, "Crime" },
                    { 11, "Comedy" },
                    { 12, "Drama" },
                    { 13, "Non-Fiction" },
                    { 14, "Biography" },
                    { 15, "Autobiography" },
                    { 16, "Memoir" },
                    { 17, "Self-Help" },
                    { 18, "Philosophy" },
                    { 19, "Psychology" },
                    { 20, "Science" },
                    { 21, "Technology" },
                    { 22, "Business" },
                    { 23, "Economics" },
                    { 24, "History" },
                    { 25, "Politics" },
                    { 26, "Sociology" },
                    { 27, "Travel" },
                    { 28, "Poetry" },
                    { 29, "Anthology" },
                    { 30, "Children's" },
                    { 31, "Young Adult (YA)" },
                    { 32, "Middle Grade" },
                    { 33, "Graphic Novel" },
                    { 34, "Comic Book" },
                    { 35, "Satire" },
                    { 36, "Dystopian" },
                    { 37, "Utopian" },
                    { 38, "Paranormal" },
                    { 39, "Supernatural" },
                    { 40, "Historical Romance" }
                });

            migrationBuilder.InsertData(
                table: "maturity_ratings",
                columns: new[] { "Id", "Name", "ShortName" },
                values: new object[,]
                {
                    { 1, "Kids", "K" },
                    { 2, "Teens", "T" },
                    { 3, "Mature", "M" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_app_reports_UserId",
                table: "app_reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_author_follows_AuthorUserId",
                table: "author_follows",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_author_follows_UserId",
                table: "author_follows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_UserId",
                table: "comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_chapters_EBookId",
                table: "ebook_chapters",
                column: "EBookId");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_genres_EBookId",
                table: "ebook_genres",
                column: "EBookId");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_genres_GenreId",
                table: "ebook_genres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_promotions_EBookId",
                table: "ebook_promotions",
                column: "EBookId");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_promotions_IsInProgress",
                table: "ebook_promotions",
                column: "IsInProgress");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_promotions_UserId",
                table: "ebook_promotions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_ratings_EBookId",
                table: "ebook_ratings",
                column: "EBookId");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_ratings_UserId",
                table: "ebook_ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_reports_EBookId",
                table: "ebook_reports",
                column: "EBookId");

            migrationBuilder.CreateIndex(
                name: "IX_ebook_reports_ReportDetailsId",
                table: "ebook_reports",
                column: "ReportDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ebooks_AuthorId",
                table: "ebooks",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ebooks_CoverArtId",
                table: "ebooks",
                column: "CoverArtId");

            migrationBuilder.CreateIndex(
                name: "IX_ebooks_MaturityRatingId",
                table: "ebooks",
                column: "MaturityRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_ebooks_Price",
                table: "ebooks",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "IX_ebooks_PublishedDate",
                table: "ebooks",
                column: "PublishedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ebooks_RatingAverage",
                table: "ebooks",
                column: "RatingAverage");

            migrationBuilder.CreateIndex(
                name: "IX_ebooks_Title",
                table: "ebooks",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_ebooks_UpdatedDate",
                table: "ebooks",
                column: "UpdatedDate");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_ebooks_EBookId",
                table: "favorite_ebooks",
                column: "EBookId");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_ebooks_UserId",
                table: "favorite_ebooks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_genres_Name",
                table: "genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_maturity_ratings_Name",
                table: "maturity_ratings",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notes_EBookChapterId",
                table: "notes",
                column: "EBookChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_notes_UserId",
                table: "notes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_EBookId",
                table: "orders",
                column: "EBookId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_OrderId",
                table: "orders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_PayeeId",
                table: "orders",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_PayerId",
                table: "orders",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_report_details_ReportReasonId",
                table: "report_details",
                column: "ReportReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_report_details_UserId",
                table: "report_details",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_report_reasons_Reason",
                table: "report_reasons",
                column: "Reason",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_bans_AdminId",
                table: "user_bans",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_user_bans_UserId",
                table: "user_bans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_libraries_EBookId",
                table: "user_libraries",
                column: "EBookId");

            migrationBuilder.CreateIndex(
                name: "IX_user_libraries_IsRead",
                table: "user_libraries",
                column: "IsRead");

            migrationBuilder.CreateIndex(
                name: "IX_user_libraries_LastChapterId",
                table: "user_libraries",
                column: "LastChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_user_libraries_UserId",
                table: "user_libraries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_libraries_UserLibraryCategoryId",
                table: "user_libraries",
                column: "UserLibraryCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_user_library_categories_Name",
                table: "user_library_categories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_user_library_categories_UserId",
                table: "user_library_categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_reports_ReportDetailsId",
                table: "user_reports",
                column: "ReportDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_user_reports_ReportedUserId",
                table: "user_reports",
                column: "ReportedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_ProfileImageId",
                table: "users",
                column: "ProfileImageId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Username",
                table: "users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_reports");

            migrationBuilder.DropTable(
                name: "author_follows");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "ebook_genres");

            migrationBuilder.DropTable(
                name: "ebook_promotions");

            migrationBuilder.DropTable(
                name: "ebook_ratings");

            migrationBuilder.DropTable(
                name: "ebook_reports");

            migrationBuilder.DropTable(
                name: "favorite_ebooks");

            migrationBuilder.DropTable(
                name: "notes");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "user_bans");

            migrationBuilder.DropTable(
                name: "user_libraries");

            migrationBuilder.DropTable(
                name: "user_reports");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "ebook_chapters");

            migrationBuilder.DropTable(
                name: "user_library_categories");

            migrationBuilder.DropTable(
                name: "report_details");

            migrationBuilder.DropTable(
                name: "ebooks");

            migrationBuilder.DropTable(
                name: "report_reasons");

            migrationBuilder.DropTable(
                name: "maturity_ratings");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "images");
        }
    }
}
