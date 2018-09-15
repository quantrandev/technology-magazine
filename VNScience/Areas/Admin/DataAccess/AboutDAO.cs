using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.DataAccess
{
    public class AboutDAO
    {
        ApplicationDbContext _db;
        public AboutDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public About Get(int id)
        {
            return _db.Abouts.Find(id);
        }
        public List<About> GetAll()
        {
            return _db.Abouts
                .OrderByDescending(e=>e.CreatedAt)
                .ToList();
        }
        public List<About> GetAllWithUser()
        {
            return _db.Abouts
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        //CRUD
        public bool Insert(About about)
        {
            bool isSuccess = true;
            try
            {
                _db.Abouts.Add(about);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Update(About about)
        {
            bool isSuccess = true;
            try
            {
                _db.Entry(about).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Destroy(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.Abouts.Remove(_db.Abouts.Find(id));
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
    }
}