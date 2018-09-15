using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.DataAccess
{
    public class ClientPostCategoryDAO
    {
        ApplicationDbContext _db;
        public ClientPostCategoryDAO(ApplicationDbContext db)
        {
            _db = db;
        }


        public PostCategory Get(int id)
        {
            return _db.PostCategories
                .Find(id);
        }
    }
}