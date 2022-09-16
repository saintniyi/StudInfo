using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudInfoDataAccess.Data;
using StudInfoDataAccess.IRepository;



namespace StudInfoDataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private AppDbContext _db;

        public IFacultyRepository Faculty { get; private set; }
        public IDeptRepository Dept { get; private set; }
        public IAppUserRepository AppUser { get; private set; }
        public ICourseRepository Course { get; private set; }
        public IStudentRepository Student { get; private set; }
        public IEnrollmentRepository Enrollment { get; private set; }
        public IScoreRepository Score { get; private set; }
        public IGradeRepository Grade { get; private set; }
        public IGpaRepository Gpa { get; private set; }
        public ICgpaRepository Cgpa { get; private set; }


        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Faculty = new FacultyRepository(_db);
            Dept = new DeptRepository(_db);
            AppUser = new AppUserRepository(_db);
            Course = new CourseRepository(_db);
            Student = new StudentRepository(_db);
            Enrollment = new EnrollmentRepository(_db);
            Score = new ScoreRepository(_db);
            Grade = new GradeRepository(_db);
            Gpa = new GpaRepository(_db);
            Cgpa = new CgpaRepository(_db);
        }


        public int GetNextValue()
        {
            var p = new SqlParameter("@result", System.Data.SqlDbType.Int);
            p.Direction = System.Data.ParameterDirection.Output;
            _db.Database.ExecuteSqlRaw("set @result = next value for GetSequenceNos", p);
            var nextVal = (int)p.Value;

            return nextVal;
        }


        public void Save()
        {
            _db.SaveChanges();
        }




    }
}
