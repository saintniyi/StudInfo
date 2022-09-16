using StudInfoModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.IRepository
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        void Update(AppUser appUser);
    }
}
