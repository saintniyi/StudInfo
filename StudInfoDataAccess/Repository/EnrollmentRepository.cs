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
    public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        private AppDbContext _db;

        public EnrollmentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Enrollment enrollment)
        {
            var enrollmentFromDb = _db.Enrollments.FirstOrDefault(x => x.ID == enrollment.ID);
            if (enrollmentFromDb != null)
            {
                enrollmentFromDb.GroupCode = enrollment.GroupCode;
                enrollmentFromDb.CourseCode = enrollment.CourseCode;
                enrollmentFromDb.StudNos = enrollment.StudNos;
                enrollmentFromDb.Level = enrollment.Level;
                enrollmentFromDb.Semester = enrollment.Semester;
                enrollmentFromDb.Processed = enrollment.Processed;
            }

        }


    }
}
