using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CateringSite.DAL
{
    public class AdminManager
    {
        UserContext db = new UserContext();

        public List<Models.AllMenus> GetAllMenuItems()
        {

            var status = new List<Models.AllMenus>();

            using (var db = new UserContext())
            {

                status = db.Database
                          .SqlQuery<Models.AllMenus>("exec Get_AllMenuItems").ToList();
            }

            return status;
        }


    }
}