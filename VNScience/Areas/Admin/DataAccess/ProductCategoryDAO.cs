using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.Models.Core;
using System.Data.Entity;

namespace VNScience.Areas.Admin.DataAccess
{
    public class ProductCategoryDAO
    {
        ApplicationDbContext _db;
        public ProductCategoryDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public List<ProductCategory> GetAll()
        {
            return _db.ProductCategories
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.Parent)
                .Include(e=>e.Children)
                .ToList();
        }

        public List<ProductCategory> GetAllExcept(int id)
        {
            return _db.ProductCategories
                 .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.Parent)
                .Where(e => e.Id != id)
                .ToList();
        }

        public ProductCategory Get(int id)
        {
            return _db.ProductCategories
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.Parent)
                .Include(e => e.Children)
                .FirstOrDefault(e => e.Id == id);
        }

        //CRUD
        public bool Insert(ProductCategory productCategory)
        {
            bool isSuccess = true;

            try
            {
                _db.ProductCategories.Add(productCategory);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Update(ProductCategory productCategory)
        {
            bool isSuccess = true;

            try
            {
                _db.Entry(productCategory).State = EntityState.Modified;
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
                _db.ProductCategories.Remove(_db.ProductCategories.Find(id));
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