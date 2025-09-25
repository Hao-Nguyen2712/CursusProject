using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cursus.Domain.Models
{
    public class CursusDBContext : IdentityDbContext<ApplicationUser>
    {
        public CursusDBContext(DbContextOptions<CursusDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Enroll> Enrolls { get; set; }
        public virtual DbSet<InstructorSubscription> InstructorSubscriptions { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Otp> Otps { get; set; }
        public virtual DbSet<Progress> Progresses { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Subscribe> Subscribes { get; set; }
        public virtual DbSet<Trading> Tradings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Ignore legacy database-first models for Identity, as IdentityDbContext handles these automatically.
        builder.Ignore<AspNetRole>();
        builder.Ignore<AspNetRoleClaim>();
        builder.Ignore<AspNetUser>();
        builder.Ignore<AspNetUserClaim>();
        builder.Ignore<AspNetUserLogin>();
        builder.Ignore<AspNetUserRole>();
        builder.Ignore<AspNetUserToken>();

        // Configure Cart relationships - use AccountId (int) to link to Account.AccountId (int)
        builder.Entity<Cart>()
            .HasOne(c => c.Account)
            .WithMany(a => a.Carts)
            .HasForeignKey(c => c.AccountId)
            .HasPrincipalKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Cart>()
            .HasOne(c => c.Course)
            .WithMany(c => c.Carts)
            .HasForeignKey(c => c.CourseId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Comment relationships - use AccountId (int) to link to Account.AccountId (int)
        builder.Entity<Comment>()
            .HasOne(c => c.Account)
            .WithMany(a => a.Commnents)
            .HasForeignKey(c => c.AccountId)
            .HasPrincipalKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Comment>()
            .HasOne(c => c.Lession)
            .WithMany(l => l.Commnents)
            .HasForeignKey(c => c.LessionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Course relationships - use AccountId (int) to link to Account.AccountId (int)
        builder.Entity<Course>()
            .HasOne(c => c.Account)
            .WithMany(a => a.Courses)
            .HasForeignKey(c => c.AccountId)
            .HasPrincipalKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Course>()
            .HasOne(c => c.Category)
            .WithMany(c => c.Courses)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Lesson relationships
        builder.Entity<Lesson>()
            .HasOne(l => l.Course)
            .WithMany(c => c.Lessons)
            .HasForeignKey(l => l.CourseId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Enroll relationships - use AccountId (int) to link to Account.AccountId (int)
        builder.Entity<Enroll>()
            .HasOne(e => e.Account)
            .WithMany(a => a.Enrolls)
            .HasForeignKey(e => e.AccountId)
            .HasPrincipalKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Enroll>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrolls)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Rate relationships - use AccountId (int) to link to Account.AccountId (int)
        builder.Entity<Rate>()
            .HasOne(r => r.Account)
            .WithMany(a => a.Rates)
            .HasForeignKey(r => r.AccountId)
            .HasPrincipalKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Rate>()
            .HasOne(r => r.Course)
            .WithMany(c => c.Rates)
            .HasForeignKey(r => r.CourseId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Otp relationships - use AccountId (int) to link to Account.AccountId (int)
        builder.Entity<Otp>()
            .HasOne(o => o.Account)
            .WithMany(a => a.Otps)
            .HasForeignKey(o => o.AccountId)
            .HasPrincipalKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Subscribe relationships manually
        builder.Entity<Subscribe>()
            .HasOne(s => s.User)
            .WithMany(a => a.UserSubscriptions)
            .HasForeignKey(s => s.UserId)
            .HasPrincipalKey(a => a.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Subscribe>()
            .HasOne(s => s.Instructor)
            .WithMany(a => a.InstructorSubscriptions)
            .HasForeignKey(s => s.InstructorId)
            .HasPrincipalKey(a => a.Id)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure Progress relationships
        builder.Entity<Progress>()
            .HasOne(p => p.Account)
            .WithMany(a => a.Progresses)
            .HasForeignKey(p => p.AccId)
            .HasPrincipalKey(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Progress>()
            .HasOne(p => p.Lesson)
            .WithMany(l => l.Progresses)
            .HasForeignKey(p => p.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Report relationships
        builder.Entity<Report>()
            .HasOne(r => r.Course)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.CourseId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Report>()
            .HasOne(r => r.Comment)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.CmtId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Report>()
            .HasOne(r => r.Account)
            .WithMany(a => a.Reports)
            .HasForeignKey(r => r.AccountId)
            .HasPrincipalKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure InstructorSubscription relationship
        builder.Entity<InstructorSubscription>()
            .HasOne(i => i.Instructor)
            .WithOne()
            .HasForeignKey<InstructorSubscription>(i => i.InstructorId)
            .HasPrincipalKey<Account>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Discount-Course many-to-many relationship
        builder.Entity<Course>()
            .HasMany(c => c.Discounts)
            .WithMany(d => d.Courses)
            .UsingEntity(j => j.ToTable("CourseDiscounts"));

        // Configure Trading relationship - use AccountId (int) to link to Account.AccountId (int)
        builder.Entity<Trading>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Tradings)
            .HasForeignKey(t => t.AccountId)
            .HasPrincipalKey(a => a.AccountId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Account entity - AccountId is primary key (identity), Id is foreign key to AspNetUsers
        builder.Entity<Account>()
            .HasKey(a => a.AccountId); // Set AccountId as primary key

        builder.Entity<Account>()
            .Property(a => a.AccountId)
            .ValueGeneratedOnAdd(); // Ensure AccountId is auto-generated

        builder.Entity<Account>()
            .HasIndex(a => a.Id)
            .IsUnique(); // Id should be unique (references AspNetUsers.Id)

        // Configure relationship between Account and AspNetUsers
        builder.Entity<Account>()
            .HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<Account>(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure decimal precision for money fields
        builder.Entity<Account>()
            .Property(a => a.Money)
            .HasPrecision(18, 2);

        builder.Entity<Cart>()
            .Property(c => c.CartMoney)
            .HasPrecision(18, 2);

        builder.Entity<Course>()
            .Property(c => c.CourseMoney)
            .HasPrecision(18, 2);

        builder.Entity<Course>()
            .Property(c => c.Discount)
            .HasPrecision(18, 2);

        builder.Entity<Trading>()
            .Property(t => t.TdMoney)
            .HasPrecision(18, 2);
    }
}
}
