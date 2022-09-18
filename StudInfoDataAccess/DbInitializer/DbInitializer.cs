using Microsoft.EntityFrameworkCore;
using StudInfoDataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudInfoDataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly AppDbContext _db;


        public DbInitializer(AppDbContext db)
        {
            _db = db;
        }



        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
        }



    }
}
