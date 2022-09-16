using StudInfoDataAccess.Data;
using StudInfoDataAccess.IRepository;
using StudInfoModel;



namespace StudInfoDataAccess.Repository
{
    public class GpaRepository : Repository<Gpa>, IGpaRepository
    {
        private AppDbContext _db;

        public GpaRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        
        public void Update(Gpa gpa)
        {
            var gpaFromDb = _db.Gpas.FirstOrDefault(x => x.Id == gpa.Id);
            if (gpaFromDb != null)
            {
                gpaFromDb.StudNos = gpa.StudNos;
                gpaFromDb.Level = gpa.Level;
                gpaFromDb.Semester = gpa.Semester;
                gpaFromDb.TotalSemesterUnits = gpa.TotalSemesterUnits;
                gpaFromDb.TotalSemesterUnitWeight = gpa.TotalSemesterUnitWeight;
                gpaFromDb.SemesterGPA = gpa.SemesterGPA;
            }
        }
    }
}
