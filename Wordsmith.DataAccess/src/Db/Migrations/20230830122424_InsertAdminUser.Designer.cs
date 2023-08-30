﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wordsmith.DataAccess.Db;

#nullable disable

namespace Wordsmith.DataAccess.src.Db.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230830122424_InsertAdminUser")]
    partial class InsertAdminUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.AppReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("app_reports");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.AuthorFollow", b =>
                {
                    b.Property<int>("AuthorUserId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AuthorUserId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("author_follows");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("EBookChapterId")
                        .HasColumnType("int");

                    b.Property<int>("EBookId")
                        .HasColumnType("int");

                    b.Property<bool>("IsShown")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("comments");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBook", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ChapterCount")
                        .HasColumnType("int");

                    b.Property<int>("CoverArtId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("HiddenDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsHidden")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MaturityRatingId")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .HasColumnType("longtext");

                    b.Property<decimal?>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<double?>("RatingAverage")
                        .HasColumnType("double");

                    b.Property<string>("Title")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CoverArtId");

                    b.HasIndex("GenreId");

                    b.HasIndex("MaturityRatingId");

                    b.HasIndex("Price");

                    b.HasIndex("PublishedDate");

                    b.HasIndex("RatingAverage");

                    b.HasIndex("Title");

                    b.HasIndex("UpdatedDate");

                    b.ToTable("ebook");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookChapter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ChapterName")
                        .HasColumnType("longtext");

                    b.Property<int>("ChapterNumber")
                        .HasColumnType("int");

                    b.Property<int>("EBookId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EBookId");

                    b.ToTable("ebook_chapters");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookPromotion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EBookId")
                        .HasColumnType("int");

                    b.Property<bool>("IsInProgress")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("PromotionEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PromotionLength")
                        .HasColumnType("int");

                    b.Property<DateTime>("PromotionStart")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EBookId");

                    b.HasIndex("IsInProgress");

                    b.HasIndex("UserId");

                    b.ToTable("ebook_promotions");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EBookId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("RatingDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EBookId");

                    b.HasIndex("UserId");

                    b.ToTable("ebook_ratings");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EBookId")
                        .HasColumnType("int");

                    b.Property<int>("ReportDetailsId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EBookId");

                    b.HasIndex("ReportDetailsId");

                    b.ToTable("ebook_reports");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookSale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EBookId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EBookId");

                    b.HasIndex("Price");

                    b.HasIndex("PurchaseDate");

                    b.HasIndex("UserId");

                    b.ToTable("ebook_sales");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.FavoriteEBook", b =>
                {
                    b.Property<int>("EBookId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("EBookId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("favorite_ebooks");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("genres");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Format")
                        .HasColumnType("longtext");

                    b.Property<string>("Path")
                        .HasColumnType("longtext");

                    b.Property<int>("Size")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("images");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.MaturityRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("maturity_ratings");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CharBegin")
                        .HasColumnType("int");

                    b.Property<int>("CharEnd")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EBookChapterId")
                        .HasColumnType("int");

                    b.Property<int>("Page")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EBookChapterId");

                    b.HasIndex("UserId");

                    b.ToTable("notes");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.ReportDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ReportReasonId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReportReasonId");

                    b.HasIndex("UserId");

                    b.ToTable("report_details");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.ReportReason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Reason")
                        .IsUnique();

                    b.ToTable("report_reasons");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("About")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<int?>("ProfileImageId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("ProfileImageId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.UserBan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BannedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AdminId");

                    b.HasIndex("UserId");

                    b.ToTable("user_bans");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.UserLibrary", b =>
                {
                    b.Property<int>("EBookId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<bool>("IsRead")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("LastChapterId")
                        .HasColumnType("int");

                    b.Property<int>("LastPage")
                        .HasColumnType("int");

                    b.Property<string>("ReadProgress")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("SyncDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("EBookId", "UserId");

                    b.HasIndex("IsRead");

                    b.HasIndex("LastChapterId");

                    b.HasIndex("UserId");

                    b.ToTable("user_libraries");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.UserReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ReportDetailsId")
                        .HasColumnType("int");

                    b.Property<int>("ReportedUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReportDetailsId");

                    b.HasIndex("ReportedUserId");

                    b.ToTable("user_reports");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.AppReport", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.AuthorFollow", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "AuthorUser")
                        .WithMany()
                        .HasForeignKey("AuthorUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AuthorUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.Comment", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBook", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.Image", "CoverArt")
                        .WithMany()
                        .HasForeignKey("CoverArtId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.MaturityRating", "MaturityRating")
                        .WithMany()
                        .HasForeignKey("MaturityRatingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CoverArt");

                    b.Navigation("Genre");

                    b.Navigation("MaturityRating");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookChapter", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.EBook", "EBook")
                        .WithMany()
                        .HasForeignKey("EBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EBook");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookPromotion", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.EBook", "EBook")
                        .WithMany()
                        .HasForeignKey("EBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "Admin")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("EBook");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookRating", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.EBook", "EBook")
                        .WithMany()
                        .HasForeignKey("EBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EBook");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookReport", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.EBook", "ReportedEBook")
                        .WithMany()
                        .HasForeignKey("EBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.ReportDetails", "ReportDetails")
                        .WithMany()
                        .HasForeignKey("ReportDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReportDetails");

                    b.Navigation("ReportedEBook");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.EBookSale", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.EBook", "EBook")
                        .WithMany()
                        .HasForeignKey("EBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EBook");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.FavoriteEBook", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.EBook", "EBook")
                        .WithMany()
                        .HasForeignKey("EBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EBook");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.Note", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.EBookChapter", "Chapter")
                        .WithMany()
                        .HasForeignKey("EBookChapterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chapter");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.ReportDetails", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.ReportReason", "ReportReason")
                        .WithMany()
                        .HasForeignKey("ReportReasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "Reporter")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReportReason");

                    b.Navigation("Reporter");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.User", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.Image", "ProfileImage")
                        .WithMany()
                        .HasForeignKey("ProfileImageId");

                    b.Navigation("ProfileImage");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.UserBan", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "Admin")
                        .WithMany()
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.UserLibrary", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.EBook", "EBook")
                        .WithMany()
                        .HasForeignKey("EBookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.EBookChapter", "LastChapter")
                        .WithMany()
                        .HasForeignKey("LastChapterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EBook");

                    b.Navigation("LastChapter");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Wordsmith.DataAccess.Db.Entities.UserReport", b =>
                {
                    b.HasOne("Wordsmith.DataAccess.Db.Entities.ReportDetails", "ReportDetails")
                        .WithMany()
                        .HasForeignKey("ReportDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Wordsmith.DataAccess.Db.Entities.User", "ReportedUser")
                        .WithMany()
                        .HasForeignKey("ReportedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReportDetails");

                    b.Navigation("ReportedUser");
                });
#pragma warning restore 612, 618
        }
    }
}
