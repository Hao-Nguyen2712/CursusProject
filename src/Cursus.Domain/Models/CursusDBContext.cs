using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Cursus.Domain.Models
{
    public partial class CursusDBContext : DbContext
    {
        public CursusDBContext()
        {
        }

        public CursusDBContext(DbContextOptions<CursusDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Commnent> Commnents { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Enroll> Enrolls { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Otp> Otps { get; set; }
        public virtual DbSet<Progress> Progresses { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Subscribe> Subscribes { get; set; }
        public virtual DbSet<Trading> Tradings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:gr2.database.windows.net,1433;Initial Catalog=CursusDB;Persist Security Info=False;User ID=gr2;Password=Password1!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__Account__F267253EEC46DA3A");

                entity.ToTable("Account");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.Avatar)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("avatar");

                entity.Property(e => e.Bio)
                    .HasMaxLength(255)
                    .HasColumnName("bio");

                entity.Property(e => e.DateofBirth)
                    .HasColumnType("date")
                    .HasColumnName("dateofBirth");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("fullName");

                entity.Property(e => e.Gender)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.IsDelete)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("isDelete");

                entity.Property(e => e.Money)
                    .HasColumnType("money")
                    .HasColumnName("money");

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.UpLevel)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("upLevel");

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_Account_AspNetUsers");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.CartMoney)
                    .HasColumnType("money")
                    .HasColumnName("Cart_money");

                entity.Property(e => e.CourseId).HasColumnName("Course_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Cart_Account");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Cart_Course");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .HasColumnName("category_name");

                entity.Property(e => e.CategoryStatus)
                    .HasMaxLength(50)
                    .HasColumnName("category_status");
            });

            modelBuilder.Entity<Commnent>(entity =>
            {
                entity.HasKey(e => e.CmtId)
                    .HasName("PK__Commnent__0EC2A3F04DBA7DD4");

                entity.Property(e => e.CmtId).HasColumnName("Cmt_id");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.CmtContent).HasColumnName("Cmt_content");

                entity.Property(e => e.CmtDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Cmt_date");

                entity.Property(e => e.CmtLevel).HasColumnName("Cmt_level");

                entity.Property(e => e.CmtReply)
                    .HasMaxLength(255)
                    .HasColumnName("Cmt_reply");

                entity.Property(e => e.LessionId).HasColumnName("LessionID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Commnents)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Commnents_Account");

                entity.HasOne(d => d.Lession)
                    .WithMany(p => p.Commnents)
                    .HasForeignKey(d => d.LessionId)
                    .HasConstraintName("FK_Commnents_Lessons");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.CourseId).HasColumnName("Course_id");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.CategoryId).HasColumnName("Category_id");

                entity.Property(e => e.CourseAvatar)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Course_avatar");

                entity.Property(e => e.CourseDate)
                    .HasColumnType("date")
                    .HasColumnName("Course_date");

                entity.Property(e => e.CourseDescription).HasColumnName("Course_description");

                entity.Property(e => e.CourseMoney)
                    .HasColumnType("money")
                    .HasColumnName("Course_money");

                entity.Property(e => e.CourseName)
                    .HasMaxLength(255)
                    .HasColumnName("Course_name");

                entity.Property(e => e.CourseProcess).HasColumnName("Course_process");

                entity.Property(e => e.CourseRequirement).HasColumnName("Course_Requirement");

                entity.Property(e => e.CourseShortDes).HasColumnName("Course_ShortDes");

                entity.Property(e => e.CourseStatus)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Course_status");

                entity.Property(e => e.CourseWlearn).HasColumnName("Course_WLearn");

                entity.Property(e => e.Discount).HasColumnType("money");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__Course__accountI__06CD04F7");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Course_Category");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.DiscountId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DiscountID");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.IsActive)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");
            });

            modelBuilder.Entity<Enroll>(entity =>
            {
                entity.ToTable("Enroll");

                entity.Property(e => e.EnrollId).HasColumnName("Enroll_id");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.CourseId).HasColumnName("Course_id");

                entity.Property(e => e.EnrollBlock)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Enroll_block");

                entity.Property(e => e.EnrollDate)
                    .HasColumnType("date")
                    .HasColumnName("Enroll_date");

                entity.Property(e => e.EnrollFinish)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Enroll_finish");

                entity.Property(e => e.EnrollStatus)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Enroll_status");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Enrolls)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Enroll_Account");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrolls)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Enroll_Course");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasKey(e => e.LessionId)
                    .HasName("PK__Lessons__75728DA0C71F3E23");

                entity.Property(e => e.LessionId).HasColumnName("Lession_id");

                entity.Property(e => e.CourseId).HasColumnName("Course_id");

                entity.Property(e => e.LessionComments).HasColumnName("Lession_comments");

                entity.Property(e => e.LessionContent).HasColumnName("Lession_content");

                entity.Property(e => e.LessionFinish).HasColumnName("Lession_finish");

                entity.Property(e => e.LessionImage)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Lession_image");

                entity.Property(e => e.LessionTilte)
                    .HasMaxLength(255)
                    .HasColumnName("Lession_tilte");

                entity.Property(e => e.LessionType)
                    .HasMaxLength(255)
                    .HasColumnName("Lession_type");

                entity.Property(e => e.LessionVideo)
                    .IsUnicode(false)
                    .HasColumnName("Lession_video");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__Lessons__Course___6E01572D");
            });

            modelBuilder.Entity<Otp>(entity =>
            {
                entity.ToTable("OTP");

                entity.Property(e => e.OtpId).HasColumnName("OTP_ID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.OtpCode).HasColumnName("OTP_code");

                entity.Property(e => e.OtpCreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("OTP_createAt");

                entity.Property(e => e.OtpExpiresAt)
                    .HasColumnType("datetime")
                    .HasColumnName("OTP_expiresAt");

                entity.Property(e => e.OtpIsUse)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OTP_IsUse");

                entity.Property(e => e.OtpType)
                    .HasMaxLength(255)
                    .HasColumnName("OTP_Type");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Otps)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_OTP_Account");
            });

            modelBuilder.Entity<Progress>(entity =>
            {
                entity.ToTable("Progress");

                entity.Property(e => e.ProgressId).HasColumnName("ProgressID");

                entity.Property(e => e.AccId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("AccID");

                entity.Property(e => e.Finish)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LessonId).HasColumnName("LessonID");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.ToTable("Rate");

                entity.Property(e => e.RateId).HasColumnName("RateID");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.CourseId).HasColumnName("Course_id");

                entity.Property(e => e.RateContent).HasColumnName("Rate_content");

                entity.Property(e => e.RateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Rate_date");

                entity.Property(e => e.RatePoint).HasColumnName("Rate_point");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Rate_Account");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Rate_Course");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.ReportId).HasColumnName("ReportID");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.CmtId).HasColumnName("Cmt_id");

                entity.Property(e => e.CourseId).HasColumnName("Course_id");

                entity.Property(e => e.ReportDate)
                    .HasColumnType("date")
                    .HasColumnName("Report_date");

                entity.Property(e => e.ReportReasson).HasColumnName("Report_reasson");
            });

            modelBuilder.Entity<Subscribe>(entity =>
            {
                entity.HasKey(e => e.SubId);

                entity.ToTable("Subscribe");

                entity.Property(e => e.SubId).HasColumnName("Sub_ID");

                entity.Property(e => e.InstructorId)
                    .HasMaxLength(450)
                    .HasColumnName("InstructorID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(450)
                    .HasColumnName("UserID");
            });

            modelBuilder.Entity<Trading>(entity =>
            {
                entity.HasKey(e => e.TdId);

                entity.ToTable("Trading");

                entity.Property(e => e.TdId).HasColumnName("TD_ID");

                entity.Property(e => e.AccountId).HasColumnName("accountID");

                entity.Property(e => e.TdDate)
                    .HasColumnType("datetime")
                    .HasColumnName("TD_date");

                entity.Property(e => e.TdMethodPayment)
                    .HasMaxLength(255)
                    .HasColumnName("TD_methodPayment");

                entity.Property(e => e.TdMoney)
                    .HasColumnType("money")
                    .HasColumnName("TD_money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
