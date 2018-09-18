using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.DataAccess
{
    public class SlideDAO
    {
        ApplicationDbContext _db;
        public SlideDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public Slide Get(int id)
        {
            return _db.Slides.Find(id);
        }

        public List<Slide> GetAll()
        {
            return _db.Slides.OrderBy(e => e.DisplayOrder).ToList();
        }

        public List<Slide> GetAllExcept(int id)
        {
            return _db.Slides
                .Where(e => e.Id != id)
                .ToList();
        }

        //CRUD
        public bool Insert(Slide slide)
        {
            bool isSuccess = true;

            try
            {
                _db.Slides.Add(slide);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Update(Slide slide)
        {
            bool isSuccess = true;

            try
            {
                _db.Entry(slide).State = EntityState.Modified;
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
                _db.Slides.Remove(_db.Slides.Find(id));
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