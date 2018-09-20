using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.DataAccess
{
    public class AdDAO
    {
        ApplicationDbContext _db;
        public AdDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public Ad Get(int id)
        {
            return _db.Ads.Find(id);
        }

        public List<Ad> GetAll()
        {
            return _db.Ads.OrderByDescending(e => e.CreatedAt).ToList();
        }

        public List<Ad> GetAllExcept(int id)
        {
            return _db.Ads
                .Where(e => e.Id != id)
                .ToList();
        }
        
        //CRUD
        public bool Insert(Ad ad)
        {
            bool isSuccess = true;

            try
            {
                _db.Ads.Add(ad);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Update(Ad ad)
        {
            bool isSuccess = true;

            try
            {
                _db.Entry(ad).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Delete(int id)
        {
            bool isSuccess = true;

            try
            {
                _db.Ads.Remove(_db.Ads.Find(id));
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool IncreaseClickCount(int id)
        {
            bool isSuccess = true;
            try
            {
                var ad = _db.Ads.Find(id);
                ad.ClickCount = ad.ClickCount + 1;
                _db.SaveChanges();
            }
            catch(Exception e)
            {
                isSuccess = false;
            }
            return isSuccess;
        }
    }
}