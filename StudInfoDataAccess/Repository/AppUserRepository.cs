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
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        private AppDbContext _db;

        public AppUserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(AppUser appUser)
        {
            var appUserFromDb = _db.AppUsers.FirstOrDefault(x => x.Id == appUser.Id);
            if (appUserFromDb != null)
            {
                appUserFromDb.FirstName = appUser.FirstName;
                appUserFromDb.LastName = appUser.LastName;
                appUserFromDb.Email = appUser.Email;  
            }


        }
    }
}
