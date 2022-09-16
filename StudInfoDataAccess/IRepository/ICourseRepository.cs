using StudInfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.IRepository
{
    public interface ICourseRepository : IRepository<Course>
    {
        public void Update(Course course);
    }
}
