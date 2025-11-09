using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace StudentOrg_A4_Website;

public partial class StudentOrgContext : DbContext
{
    public StudentOrgContext()
    {
    }

    public StudentOrgContext(DbContextOptions<StudentOrgContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountRequest> AccountRequests { get; set; }

    public virtual DbSet<BureauMember> BureauMembers { get; set; }

    public virtual DbSet<BureauPosition> BureauPositions { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=sarmale123;database=StudentOrg", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.44-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PRIMARY");

            entity.ToTable("accounts");

            entity.HasIndex(e => e.MemberId, "member_id");

            entity.HasIndex(e => e.Email, "unique_email").IsUnique();

            entity.HasIndex(e => e.Phone, "unique_phone").IsUnique();

            entity.HasIndex(e => e.Username, "unique_username").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AccountPassword)
                .HasMaxLength(256)
                .HasColumnName("account_password");
            entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(25)
                .HasColumnName("phone");
            entity.Property(e => e.TerminationDate).HasColumnName("termination_date");
            entity.Property(e => e.Username)
                .HasMaxLength(200)
                .HasColumnName("username");

            entity.HasOne(d => d.Member).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("accounts_ibfk_1");
        });

        modelBuilder.Entity<AccountRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PRIMARY");

            entity.ToTable("account_requests");

            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.RequestAuthor)
                .HasMaxLength(200)
                .HasColumnName("request_author");
            entity.Property(e => e.RequestAuthorRole)
                .HasMaxLength(50)
                .HasColumnName("request_author_role");
            entity.Property(e => e.RequestDate).HasColumnName("request_date");
            entity.Property(e => e.RequestDescription)
                .HasMaxLength(500)
                .HasColumnName("request_description");
            entity.Property(e => e.RequestStatus)
                .HasMaxLength(10)
                .HasColumnName("request_status");
            entity.Property(e => e.RequestedFirstName)
                .HasMaxLength(100)
                .HasColumnName("requested_first_name");
            entity.Property(e => e.RequestedLastName)
                .HasMaxLength(100)
                .HasColumnName("requested_last_name");
        });

        modelBuilder.Entity<BureauMember>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("bureau_members");

            entity.HasIndex(e => e.MemberId, "member_id");

            entity.HasIndex(e => e.PositionId, "position_id");

            entity.Property(e => e.EndTermDate).HasColumnName("end_term_date");
            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.StartTermDate).HasColumnName("start_term_date");

            entity.HasOne(d => d.Member).WithMany()
                .HasForeignKey(d => d.MemberId)
                .HasConstraintName("bureau_members_ibfk_1");

            entity.HasOne(d => d.Position).WithMany()
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("bureau_members_ibfk_2");
        });

        modelBuilder.Entity<BureauPosition>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PRIMARY");

            entity.ToTable("bureau_positions");

            entity.HasIndex(e => e.PositionAlias, "unique_alias").IsUnique();

            entity.HasIndex(e => e.PositionName, "unique_name").IsUnique();

            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.PositionAlias)
                .HasMaxLength(30)
                .HasColumnName("position_alias");
            entity.Property(e => e.PositionName)
                .HasMaxLength(50)
                .HasColumnName("position_name");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PRIMARY");

            entity.ToTable("departments");

            entity.HasIndex(e => e.DepartmentAlias, "unique_alias").IsUnique();

            entity.HasIndex(e => e.DepartmentName, "unique_name").IsUnique();

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentAlias)
                .HasMaxLength(10)
                .HasColumnName("department_alias");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .HasColumnName("department_name");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PRIMARY");

            entity.ToTable("members");

            entity.Property(e => e.MemberId).HasColumnName("member_id");
            entity.Property(e => e.Faculty)
                .HasMaxLength(150)
                .HasColumnName("faculty");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.JoinDate).HasColumnName("join_date");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.LeaveDate).HasColumnName("leave_date");

            entity.HasMany(d => d.Departments).WithMany(p => p.Members)
                .UsingEntity<Dictionary<string, object>>(
                    "DepartmentMember",
                    r => r.HasOne<Department>().WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("department_members_ibfk_2"),
                    l => l.HasOne<Member>().WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("department_members_ibfk_1"),
                    j =>
                    {
                        j.HasKey("MemberId", "DepartmentId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("department_members");
                        j.HasIndex(new[] { "DepartmentId" }, "department_id");
                        j.IndexerProperty<int>("MemberId").HasColumnName("member_id");
                        j.IndexerProperty<int>("DepartmentId").HasColumnName("department_id");
                    });
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasKey(e => e.PictureId).HasName("PRIMARY");

            entity.ToTable("pictures");

            entity.Property(e => e.PictureId).HasColumnName("picture_id");
            entity.Property(e => e.Location)
                .HasMaxLength(500)
                .HasColumnName("location");

            entity.HasMany(d => d.Posts).WithMany(p => p.Pictures)
                .UsingEntity<Dictionary<string, object>>(
                    "PostsPicture",
                    r => r.HasOne<Post>().WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("posts_pictures_ibfk_2"),
                    l => l.HasOne<Picture>().WithMany()
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("posts_pictures_ibfk_1"),
                    j =>
                    {
                        j.HasKey("PictureId", "PostId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("posts_pictures");
                        j.HasIndex(new[] { "PostId" }, "post_id");
                        j.IndexerProperty<int>("PictureId").HasColumnName("picture_id");
                        j.IndexerProperty<int>("PostId").HasColumnName("post_id");
                    });
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PRIMARY");

            entity.ToTable("posts");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.PostAuthor)
                .HasMaxLength(200)
                .HasColumnName("post_author");
            entity.Property(e => e.PostBanner)
                .HasMaxLength(500)
                .HasColumnName("post_banner");
            entity.Property(e => e.PostContent)
                .HasColumnType("text")
                .HasColumnName("post_content");
            entity.Property(e => e.PostDescription)
                .HasMaxLength(500)
                .HasColumnName("post_description");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
