using StudInfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.IRepository
{
    public interface IGradeRepository : IRepository<Grade> 
    {
        void Update(Grade grade);

    }
}
