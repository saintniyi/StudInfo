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
    public class DeptRepository : Repository<Dept>, IDeptRepository
    {
        private AppDbContext _db;

        public DeptRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Dept dept)
        {
            //_db.Update(dept);

            var deptFromDb = _db.Depts.FirstOrDefault(x => x.ID == dept.ID);
            if (deptFromDb != null)
            {
                deptFromDb.DeptName = dept.DeptName;
                deptFromDb.FacultyCode = dept.FacultyCode;
            }
        }


    }
}
