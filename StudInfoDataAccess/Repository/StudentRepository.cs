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
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private AppDbContext _db;

        public StudentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Student student)
        {
            var studentFromDb = _db.Students.FirstOrDefault(x => x.Id == student.Id);
            if (studentFromDb != null)
            {
                studentFromDb.FirstName = student.FirstName;
                studentFromDb.LastName = student.LastName;
                studentFromDb.Email = student.Email;
                studentFromDb.PhoneNos = student.PhoneNos;
                studentFromDb.DOB = student.DOB;
                studentFromDb.AdmissionDate = student.AdmissionDate;
                studentFromDb.Sex = student.Sex;
                studentFromDb.Pix = student.Pix;
                studentFromDb.DeptCode = student.DeptCode;
                studentFromDb.DegreeProgramName = student.DegreeProgramName;
                studentFromDb.DegreeTitleToBeAwarded = student.DegreeTitleToBeAwarded;
                //studentFromDb.Level = student.Level;
                //studentFromDb.Semester = student.Semester;
                //studentFromDb.LatestEnrollGrpCode = student.LatestEnrollGrpCode;
                //studentFromDb.GPA = student.GPA;
                //studentFromDb.CGPA = student.CGPA;
                studentFromDb.Remarks = student.Remarks;
                studentFromDb.PhotoBackupPath = student.PhotoBackupPath;
            }
        }


    }
}
