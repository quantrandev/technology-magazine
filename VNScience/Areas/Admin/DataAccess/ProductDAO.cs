using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.DataAccess
{
    public class ProductDAO
    {
        ApplicationDbContext _db;
        public ProductDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public Product Get(int id)
        {
            return _db.Products
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.ProductCategory)
                .FirstOrDefault(e => e.Id == id);
        }

        public List<Product> GetAll()
        {
            return _db.Products.ToList();
        }

        public List<Product> GetAllExcept(int id)
        {
            return _db.Products
                .Where(e => e.Id != id)
                .ToList();
        }

        //CRUD
        public bool Insert(Product product)
        {
            bool isSuccess = true;

            try
            {
                _db.Products.Add(product);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Update(Product product)
        {
            bool isSuccess = true;

            try
            {
                _db.Entry(product).State = EntityState.Modified;
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
                _db.Products.Remove(_db.Products.Find(id));
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