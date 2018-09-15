using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.DataAccess
{
    public class PostCategoryDAO
    {
        ApplicationDbContext _db;
        public PostCategoryDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        //GET
        public List<PostCategory> GetAll()
        {
            return _db.PostCategories
                .OrderBy(e=>e.DisplayOrder)
                .ToList();
        }

        public List<PostCategory> GetManyRequestsToDelete()
        {
            return _db.PostCategories
                .Where(e => e.IsRequestedDelete.Value)
                .OrderByDescending(e=>e.CreatedAt)
                .OrderByDescending(e => e.UpdatedAt)
                .ToList();
        }

        public int GetRequestsToDeleteCount()
        {
            return _db.PostCategories.Where(e => e.IsRequestedDelete.Value).Count();
        }
        
        //ENDGET

        public bool MarkAsDelete(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.PostCategories.Find(id).IsRequestedDelete = true;
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
                _db.PostCategories.Remove(_db.PostCategories.Find(id));
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }


        public bool UnDeleteRequest(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.PostCategories.Find(id).IsRequestedDelete = false;
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