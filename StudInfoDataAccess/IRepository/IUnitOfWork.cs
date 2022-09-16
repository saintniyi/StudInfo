using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.IRepository
{
    public interface IUnitOfWork
    {
        public IFacultyRepository Faculty { get; }

        public IDeptRepository Dept { get; }

        public IAppUserRepository AppUser { get; }

        public ICourseRepository Course { get; }

        public IStudentRepository Student { get; }

        public IEnrollmentRepository Enrollment { get; }
               

        public IGradeRepository Grade { get; }


        public IScoreRepository Score { get; }


        public IGpaRepository Gpa { get; }


        public ICgpaRepository Cgpa { get; }


        public int GetNextValue();


        void Save();

    }
}
