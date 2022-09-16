using StudInfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.IRepository
{
    public interface ICgpaRepository : IRepository<Cgpa> 
    {
        void Update(Cgpa cgpa);

    }
}
