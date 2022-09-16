using StudInfoDataAccess.Data;
using StudInfoDataAccess.IRepository;
using StudInfoModel;



namespace StudInfoDataAccess.Repository
{
    public class CgpaRepository : Repository<Cgpa>, ICgpaRepository
    {
        private AppDbContext _db;

        public CgpaRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        
        public void Update(Cgpa cgpa)
        {
            var cgpaFromDb = _db.Cgpas.FirstOrDefault(x => x.Id == cgpa.Id);
            if (cgpaFromDb != null)
            {
                cgpaFromDb.StudNos = cgpa.StudNos;
                cgpaFromDb.TotalUnits = cgpa.TotalUnits;
                cgpaFromDb.TotalUnitWeight = cgpa.TotalUnitWeight;
                cgpaFromDb.CumulativeGPA = cgpa.CumulativeGPA;
            }
        }
    }
}
