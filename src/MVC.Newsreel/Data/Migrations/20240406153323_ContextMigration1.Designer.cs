﻿// <auto-generated />
using System;
using MVC.Newsreel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MVC.Newsreel.Data.Migrations
{
    [DbContext(typeof(Lab1dbContext))]
    [Migration("20240406153323_ContextMigration1")]
    partial class ContextMigration1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MVC.Newsreel.Data.Article", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ArticleID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArticleId"));

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("AuthorID");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("CategoryID");

                    b.Property<int?>("Dislikes")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<int?>("Likes")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PubDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ArticleId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Article", (string)null);
                });

            modelBuilder.Entity("MVC.Newsreel.Data.ArticleDraft", b =>
                {
                    b.Property<int>("ArticleDraftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ArticleDraftID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArticleDraftId"));

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("AuthorID");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<int?>("SuggestedCategoryId")
                        .HasColumnType("int")
                        .HasColumnName("SuggestedCategoryID");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ArticleDraftId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("SuggestedCategoryId");

                    b.ToTable("ArticleDraft", (string)null);
                });

            modelBuilder.Entity("MVC.Newsreel.Data.ArticleRequest", b =>
                {
                    b.Property<int>("ArticleRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ArticleRequestID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArticleRequestId"));

                    b.Property<int?>("ArticleDraftId")
                        .HasColumnType("int")
                        .HasColumnName("ArticleDraftID");

                    b.Property<int?>("ArticleId")
                        .HasColumnType("int")
                        .HasColumnName("ArticleID");

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("AuthorID");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("ArticleRequestId");

                    b.HasIndex("ArticleDraftId");

                    b.HasIndex("ArticleId");

                    b.HasIndex("AuthorId");

                    b.ToTable("ArticleRequest", (string)null);
                });

            modelBuilder.Entity("MVC.Newsreel.Data.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CategoryID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CategoryId");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("MVC.Newsreel.Data.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CommentID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<int?>("ArticleId")
                        .HasColumnType("int")
                        .HasColumnName("ArticleID");

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("AuthorID");

                    b.Property<int>("Dislikes")
                        .HasColumnType("int");

                    b.Property<int>("Likes")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PubDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("CommentId");

                    b.HasIndex("ArticleId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Comment", (string)null);
                });

            modelBuilder.Entity("MVC.Newsreel.Data.Like", b =>
                {
                    b.Property<int>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("LikeID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LikeId"));

                    b.Property<int?>("ArticleId")
                        .HasColumnType("int")
                        .HasColumnName("ArticleID");

                    b.Property<int?>("CommentId")
                        .HasColumnType("int")
                        .HasColumnName("CommentID");

                    b.Property<bool>("IsDis")
                        .HasColumnType("bit")
                        .HasColumnName("is_dis");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("LikeId");

                    b.HasIndex("ArticleId");

                    b.HasIndex("CommentId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("MVC.Newsreel.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MVC.Newsreel.Data.Article", b =>
                {
                    b.HasOne("MVC.Newsreel.Data.User", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("FK_Article_Author");

                    b.HasOne("MVC.Newsreel.Data.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_Article_Category");

                    b.Navigation("Author");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("MVC.Newsreel.Data.ArticleDraft", b =>
                {
                    b.HasOne("MVC.Newsreel.Data.User", "Author")
                        .WithMany("ArticleDrafts")
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("FK_ArticleDraft_Author");

                    b.HasOne("MVC.Newsreel.Data.Category", "SuggestedCategory")
                        .WithMany("ArticleDrafts")
                        .HasForeignKey("SuggestedCategoryId")
                        .HasConstraintName("FK_ArticleDraft_Category");

                    b.Navigation("Author");

                    b.Navigation("SuggestedCategory");
                });

            modelBuilder.Entity("MVC.Newsreel.Data.ArticleRequest", b =>
                {
                    b.HasOne("MVC.Newsreel.Data.ArticleDraft", "ArticleDraft")
                        .WithMany("ArticleRequests")
                        .HasForeignKey("ArticleDraftId")
                        .HasConstraintName("FK_ArticleRequest_ArticleDraft");

                    b.HasOne("MVC.Newsreel.Data.Article", "Article")
                        .WithMany("ArticleRequests")
                        .HasForeignKey("ArticleId")
                        .HasConstraintName("FK_ArticleRequest_Article");

                    b.HasOne("MVC.Newsreel.Data.User", "Author")
                        .WithMany("ArticleRequests")
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("FK_ArticleRequest_Author");

                    b.Navigation("Article");

                    b.Navigation("ArticleDraft");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MVC.Newsreel.Data.Comment", b =>
                {
                    b.HasOne("MVC.Newsreel.Data.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .HasConstraintName("FK_Comment_Article");

                    b.HasOne("MVC.Newsreel.Data.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("FK_Comment_Author");

                    b.Navigation("Article");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MVC.Newsreel.Data.Like", b =>
                {
                    b.HasOne("MVC.Newsreel.Data.Article", "Article")
                        .WithMany("LikesNavigation")
                        .HasForeignKey("ArticleId")
                        .HasConstraintName("FK_Article_Like");

                    b.HasOne("MVC.Newsreel.Data.Comment", "Comment")
                        .WithMany("LikesNavigation")
                        .HasForeignKey("CommentId")
                        .HasConstraintName("FK_Comment_Like");

                    b.HasOne("MVC.Newsreel.Data.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_User_Like");

                    b.Navigation("Article");

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("MVC.Newsreel.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("MVC.Newsreel.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MVC.Newsreel.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("MVC.Newsreel.Data.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MVC.Newsreel.Data.Article", b =>
                {
                    b.Navigation("ArticleRequests");

                    b.Navigation("Comments");

                    b.Navigation("LikesNavigation");
                });

            modelBuilder.Entity("MVC.Newsreel.Data.ArticleDraft", b =>
                {
                    b.Navigation("ArticleRequests");
                });

            modelBuilder.Entity("MVC.Newsreel.Data.Category", b =>
                {
                    b.Navigation("ArticleDrafts");

                    b.Navigation("Articles");
                });

            modelBuilder.Entity("MVC.Newsreel.Data.Comment", b =>
                {
                    b.Navigation("LikesNavigation");
                });

            modelBuilder.Entity("MVC.Newsreel.Data.User", b =>
                {
                    b.Navigation("ArticleDrafts");

                    b.Navigation("ArticleRequests");

                    b.Navigation("Articles");

                    b.Navigation("Comments");

                    b.Navigation("Likes");
                });
#pragma warning restore 612, 618
        }
    }
}
