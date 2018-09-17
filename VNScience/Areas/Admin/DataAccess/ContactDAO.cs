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
    public class ContactDAO
    {
        ApplicationDbContext _db;
        public ContactDAO(ApplicationDbContext db)
        {
            this._db = db;
        }

        public List<Contact> GetAll()
        {
            return _db.Contacts.OrderBy(e => e.IsSeen).ToList();
        }

        public bool MarkAsSeen(long id)
        {
            bool isSuccess = true;

            try
            {
                _db.Contacts.Find(id).IsSeen = true;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }


        public bool Destroy(long id)
        {
            bool isSuccess = true;

            try
            {
                _db.Contacts.Remove(_db.Contacts.Find(id));
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        //search
        public List<Contact> Search(string searchString)
        {
            var searchTerms = StringHelper.FilterWhiteSpaces(searchString).Trim().Split(' ');

            var query = _db.Contacts.AsQueryable();

            //exactly match
            var predicate = PredicateBuilder.New<Contact>();
            predicate = predicate.Or(e => e.Title.Contains(searchString));
            predicate = predicate.Or(e => e.Message.Contains(searchString));

            //partial match
            foreach (var item in searchTerms)
            {
                if (item.Length != 1)
                {
                    predicate = predicate.Or(e => e.Title.Contains(item));
                    predicate = predicate.Or(e => e.Message.Contains(item));
                }
            }

            return query.Where(predicate)
                .OrderBy(e => e.IsSeen)
                 .ToList();
        }
    }
}