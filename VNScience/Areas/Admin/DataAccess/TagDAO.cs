using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.Models.Core;
using System.Data.Entity;

namespace VNScience.Areas.Admin.DataAccess
{
    public class TagDAO
    {
        ApplicationDbContext _db;
        public TagDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public Tag Get(string id)
        {
            return _db.Tags.Find(id);
        }
        public List<Tag> GetAll()
        {
            return _db.Tags.ToList();
        }

        public List<Tag> GetByPost(long id)
        {
            return _db.Tags
                .Include(e=>e.Posts)
                .Where(e => e.Posts.Select(p => p.Id).Contains(id))
                .ToList();
        }
    }
}