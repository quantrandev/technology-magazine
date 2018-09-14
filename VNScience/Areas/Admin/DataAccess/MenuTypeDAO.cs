using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.DataAccess
{
    public class MenuTypeDAO
    {
        ApplicationDbContext _db;
        public MenuTypeDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public List<MenuType> GetAll()
        {
            return _db.MenuTypes.ToList();
        }
    }
}