using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace united_movers_api.Models;

public partial class UnitedMoversDbContext : DbContext
{
    public UnitedMoversDbContext()
    {
    }

    public UnitedMoversDbContext(DbContextOptions<UnitedMoversDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermissionMapping> RolePermissionMappings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRoleMapping> UserRoleMappings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB0F581435DB");

            entity.HasIndex(e => e.PermissionName, "UQ__Permissi__0FFDA357046F7ABB").IsUnique();

            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AC946327B");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160A3212F82").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RolePermissionMapping>(entity =>
        {
            entity.HasKey(e => e.MappingId).HasName("PK__RolePerm__8B5781BDCCC3A5C4");

            entity.ToTable("RolePermissionMapping");

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "idx_role_permission");

            entity.Property(e => e.MappingId).HasColumnName("MappingID");
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissionMappings)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__RolePermi__Permi__48CFD27E");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissionMappings)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__RolePermi__RoleI__47DBAE45");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACF68EF1F7");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E43B2A1655").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534DD9025C9").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRoleMapping>(entity =>
        {
            entity.HasKey(e => e.MappingId).HasName("PK__UserRole__8B5781BD876715FB");

            entity.ToTable("UserRoleMapping");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "idx_user_role");

            entity.Property(e => e.MappingId).HasColumnName("MappingID");
            entity.Property(e => e.AssignedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__UserRoleM__RoleI__4222D4EF");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoleMappings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRoleM__UserI__412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
