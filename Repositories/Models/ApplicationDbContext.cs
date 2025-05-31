using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Okxkey> Okxkeys { get; set; }

    public virtual DbSet<SubAccount> SubAccounts { get; set; }

    public virtual DbSet<TradeLog> TradeLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WebhookEvent> WebhookEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-IQK16CS\\QUANGZY;Database=OKXApiSystem;User ID=sa;Password=12345;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Okxkey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OKXKeys__3214EC07E5588375");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.User)
                  .WithMany(p => p.Okxkeys)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK__OKXKeys__UserId__3B75D760");
        });

        modelBuilder.Entity<SubAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SubAccou__3214EC073E840BDE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Okxkey)
                  .WithMany(p => p.SubAccounts)
                  .HasForeignKey(d => d.OkxkeyId)
                  .HasConstraintName("FK__SubAccoun__OKXKe__48CFD27E");

            entity.HasOne(d => d.ParentUser)
                  .WithMany(p => p.SubAccounts)
                  .HasForeignKey(d => d.ParentUserId)
                  .HasConstraintName("FK__SubAccoun__Paren__47DBAE45");
        });

        modelBuilder.Entity<TradeLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TradeLog__3214EC07BA2135A0");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsCurrentOrder).HasDefaultValue(false);

            entity.Property(e => e.EntryPrice).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.ExitPrice).HasColumnType("decimal(18, 8)");
            entity.Property(e => e.ExecutedAt).HasColumnType("datetime");
            entity.Property(e => e.ClosedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User)
                  .WithMany(p => p.TradeLogs)
                  .HasForeignKey(d => d.UserId)
                  .HasConstraintName("FK__TradeLogs__UserI__4CA06362");

            entity.HasOne(d => d.WebhookEvent)
                  .WithMany(p => p.TradeLogs)
                  .HasForeignKey(d => d.WebhookEventId)
                  .HasConstraintName("FK_TradeLogs_WebhookEvent");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC071CD41885");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<WebhookEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WebhookE__3214EC074C77B85B");

            entity.Property(e => e.ReceivedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
