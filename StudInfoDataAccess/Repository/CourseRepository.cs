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
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private AppDbContext _db;

        public CourseRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Course course)
        {
            var courseFromDb = _db.Courses.FirstOrDefault(x => x.Id == course.Id);
            if (courseFromDb != null)
            {
                courseFromDb.CourseName = course.CourseName;
                courseFromDb.CourseUnit = course.CourseUnit;
                courseFromDb.DeptCode = course.DeptCode;
                courseFromDb.Semester = course.Semester;
                courseFromDb.Level = course.Level;
            }
        }


    }
}
