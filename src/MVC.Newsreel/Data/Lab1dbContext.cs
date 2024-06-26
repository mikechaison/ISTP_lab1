﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MVC.Newsreel.Data;

public partial class Lab1dbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public Lab1dbContext()
    {
    }

    public Lab1dbContext(DbContextOptions<Lab1dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleDraft> ArticleDrafts { get; set; }

    public virtual DbSet<ArticleRequest> ArticleRequests { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("Article");

            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.PubDate).HasColumnType("datetime");
            entity.Property(e => e.Text).HasColumnType("text");
            entity.Property(e => e.Title).HasColumnType("text");

            entity.HasOne(d => d.Author).WithMany(p => p.Articles)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Article_Author");

            entity.HasOne(d => d.Category).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Article_Category");
        });

        modelBuilder.Entity<ArticleDraft>(entity =>
        {
            entity.ToTable("ArticleDraft");

            entity.Property(e => e.ArticleDraftId).HasColumnName("ArticleDraftID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.SuggestedCategoryId).HasColumnName("SuggestedCategoryID");
            entity.Property(e => e.Text).HasColumnType("text");
            entity.Property(e => e.Title).HasColumnType("text");

            entity.HasOne(d => d.Author).WithMany(p => p.ArticleDrafts)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleDraft_Author");

            entity.HasOne(d => d.SuggestedCategory).WithMany(p => p.ArticleDrafts)
                .HasForeignKey(d => d.SuggestedCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleDraft_Category");
        });

        modelBuilder.Entity<ArticleRequest>(entity =>
        {
            entity.ToTable("ArticleRequest");

            entity.Property(e => e.ArticleRequestId).HasColumnName("ArticleRequestID");
            entity.Property(e => e.ArticleDraftId).HasColumnName("ArticleDraftID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ArticleDraft).WithMany(p => p.ArticleRequests)
                .HasForeignKey(d => d.ArticleDraftId)
                .HasConstraintName("FK_ArticleRequest_ArticleDraft");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleRequests)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_ArticleRequest_Article");

            entity.HasOne(d => d.Author).WithMany(p => p.ArticleRequests)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ArticleRequest_Author");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.PubDate).HasColumnType("datetime");
            entity.Property(e => e.Text).HasColumnType("text");

            entity.HasOne(d => d.Article).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Article");

            entity.HasOne(d => d.Author).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comment_Author");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.Property(e => e.LikeId).HasColumnName("LikeID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.IsDis).HasColumnName("is_dis");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Article).WithMany(p => p.LikesNavigation)
                .HasForeignKey(d => d.ArticleId)
                .HasConstraintName("FK_Article_Like");

            entity.HasOne(d => d.Comment).WithMany(p => p.LikesNavigation)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK_Comment_Like");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Like");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
