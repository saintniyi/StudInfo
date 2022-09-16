using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudInfoModel;

namespace StudInfoDataAccess.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Dept> Depts { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Gpa> Gpas { get; set; }
    public DbSet<Cgpa> Cgpas { get; set; }
    public DbSet<Score> Scores { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);


        //**the snippet below is a new addition to this method to change cascade behaviour**//
        //Due to this addition, migration: changeCascadeBehavior
        //was added through the package manager console
        foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }

        //Create sequence here
        builder.HasSequence("GetSequenceNos", x => x.StartsAt(1).IncrementsBy(1));


        //Configure student properties here
        //builder.Entity<Student>().HasIndex(i => i.StudNos).IsUnique();
        builder.Entity<Student>().Property(x => x.Id).UseIdentityColumn();

        builder.Entity<Student>().Property(i => i.Sex)
            .HasMaxLength(10)
            .HasConversion(i => i.ToString(),
             i => (Gender)Enum.Parse(typeof(Gender), i));


        builder.Entity<Student>().Property(i => i.DegreeProgramName)
            .HasMaxLength(120)
            .HasConversion(i => i.ToString(),
             i => (ProgramName)Enum.Parse(typeof(ProgramName), i));


        builder.Entity<Student>().Property(i => i.DegreeTitleToBeAwarded)
            .HasMaxLength(120)
            .HasConversion(i => i.ToString(),
             i => (DegreeTitle)Enum.Parse(typeof(DegreeTitle), i));

    }
}
