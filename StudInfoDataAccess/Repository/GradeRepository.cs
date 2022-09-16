using StudInfoDataAccess.Data;
using StudInfoDataAccess.IRepository;
using StudInfoModel;



namespace StudInfoDataAccess.Repository
{
    public class GradeRepository : Repository<Grade>, IGradeRepository
    {
        private AppDbContext _db;

        public GradeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        
        public void Update(Grade grade)
        {
            var gradeFromDb = _db.Grades.FirstOrDefault(x => x.Id == grade.Id);
            if (gradeFromDb != null)
            {
                gradeFromDb.MinScoreRange = grade.MinScoreRange;
                gradeFromDb.MaxScoreRange = grade.MaxScoreRange;
                gradeFromDb.Weight = grade.Weight;
            }
        }
    }
}
