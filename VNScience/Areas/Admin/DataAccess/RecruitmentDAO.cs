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
    public class RecruitmentDAO
    {
        ApplicationDbContext _db;

        public RecruitmentDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public List<Recruitment> GetAllWithUser()
        {
            return _db.Recruitments
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        public Recruitment GetWithUser(long id)
        {
            return _db.Recruitments
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .FirstOrDefault(e => e.Id == id);                
        }

        public Recruitment Get(long id)
        {
            return _db.Recruitments.Find(id);
        }

        //CRUD
        public bool Insert(Recruitment recruitment)
        {
            bool isSuccess = true;

            try
            {
                _db.Recruitments.Add(recruitment);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Update(Recruitment recruitment)
        {
            bool isSuccess = true;

            try
            {
                _db.Entry(recruitment).State = EntityState.Modified;
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
                _db.Recruitments.Remove(_db.Recruitments.Find(id));
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        //search
        public List<Recruitment> Search(string searchString)
        {
            var searchTerms = StringHelper.FilterWhiteSpaces(searchString).Trim().Split(' ');

            var query = _db.Recruitments.AsQueryable();

            //eager loading
            query = query.Include(e => e.CreatingUser);
            query = query.Include(e => e.UpdatingUser);

            //exactly match
            var predicate = PredicateBuilder.New<Recruitment>();
            predicate = predicate.Or(e => e.Title.Contains(searchString));
            predicate = predicate.Or(e => e.JobTitle.Contains(searchString));
            predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(searchString));
            predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(searchString));

            //partial match
            foreach (var item in searchTerms)
            {
                if (item.Length != 1)
                {
                    predicate = predicate.Or(e => e.Title.Contains(item));
                    predicate = predicate.Or(e => e.JobTitle.Contains(searchString));
                    predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(item));
                    predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(item));
                }
            }

            return query.Where(predicate)
                 .ToList();
        }
    }
}