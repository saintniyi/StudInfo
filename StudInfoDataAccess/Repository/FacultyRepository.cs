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
    public class FacultyRepository : Repository<Faculty>, IFacultyRepository
    {
        private AppDbContext _db;

        public FacultyRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        //public void Save()
        //{
        //    _db.SaveChanges();
        //}

        public void Update(Faculty faculty)
        {
            //_db.Faculties.Update(faculty);

            var facultyFromDb = _db.Faculties.FirstOrDefault(x => x.FacultyCode == faculty.FacultyCode);
            if (facultyFromDb == null)
            {
                facultyFromDb.FacultyName = faculty.FacultyName;
            }

        }
    }
}
