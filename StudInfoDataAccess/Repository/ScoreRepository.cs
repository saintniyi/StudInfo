using StudInfoDataAccess.Data;
using StudInfoDataAccess.IRepository;
using StudInfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.Repository
{
    public class ScoreRepository : Repository<Score>, IScoreRepository
    {
        private AppDbContext _db;

        public ScoreRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Score score)
        {
            var scoreFromDb = _db.Scores.FirstOrDefault(x => x.ID == score.ID);
            if (scoreFromDb != null)
            {
                scoreFromDb.GroupCode = score.GroupCode;
                scoreFromDb.CourseCode = score.CourseCode;
                scoreFromDb.StudNos = score.StudNos;
                scoreFromDb.Mark = score.Mark;
                scoreFromDb.Level = score.Level;
                scoreFromDb.Semester = score.Semester;
                scoreFromDb.Processed = score.Processed;
                scoreFromDb.GradeLetter = score.GradeLetter;
                scoreFromDb.Weight = score.Weight;
            }

        }


    }
}
