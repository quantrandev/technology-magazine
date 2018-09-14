using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VNScience.Common;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.Areas.Admin.DataAccess
{
    public class MenuDAO
    {
        ApplicationDbContext _db;
        public MenuDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public List<Menu> GetAll()
        {
            return _db.Menus.ToList();
        }

        public Menu Get(int id)
        {
            return _db.Menus.Find(id);
        }

        public List<Menu> GetAllWithTypeAndUser()
        {
            return _db.Menus
                .Include(e => e.MenuType)
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .ToList();
        }

        public List<Menu> GetAllWithType()
        {
            return _db.Menus
                .Include(e => e.MenuType)
                .ToList();
        }

        public List<Menu> GetAllWithTypeExcept(int id)
        {
            return _db.Menus
                .Include(e => e.MenuType)
                .Where(e => e.Id != id)
                .ToList();
        }

        public List<Menu> GetAllExcept(int id)
        {
            return _db.Menus
                .Where(e => e.Id != id)
                .ToList();
        }

        //CRUD
        public bool Insert(Menu menu)
        {
            bool isSuccess = true;

            try
            {
                _db.Menus.Add(menu);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Update(Menu menu)
        {
            bool isSuccess = true;

            try
            {
                _db.Entry(menu).State = EntityState.Modified;
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
                _db.Menus.Remove(_db.Menus.Find(id));
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        //search
        public List<Menu> Search(string searchString)
        {
            var searchTerms = StringHelper.FilterWhiteSpaces(searchString).Trim().Split(' ');

            var query = _db.Menus.AsQueryable();

            //eager loading
            query = query.Include(e => e.CreatingUser);
            query = query.Include(e => e.UpdatingUser);
            query = query.Include(e => e.MenuType);

            //exactly match
            var predicate = PredicateBuilder.New<Menu>();
            predicate = predicate.Or(e => e.Title.Contains(searchString));
            predicate = predicate.Or(e => e.Link.Contains(searchString));
            predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(searchString));
            predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(searchString));

            //partial match
            foreach (var item in searchTerms)
            {
                if (item.Length != 1)
                {
                    predicate = predicate.Or(e => e.Title.Contains(item));
                    predicate = predicate.Or(e => e.Link.Contains(item));
                    predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(item));
                    predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(item));
                }
            }

            return query.Where(predicate)
                 .ToList();
        }
    }
}