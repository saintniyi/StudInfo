using StudInfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.IRepository
{
    public interface IFacultyRepository : IRepository<Faculty> 
    {
        void Update(Faculty faculty);

        //void Save();

    }
}
