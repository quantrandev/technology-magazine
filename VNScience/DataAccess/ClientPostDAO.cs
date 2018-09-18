using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VNScience.Common;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.DataAccess
{
    public class ClientPostDAO
    {
        ApplicationDbContext _db;
        public ClientPostDAO(ApplicationDbContext db)
        {
            _db = db;
        }

        public Post Get(long id)
        {
            return _db.Posts
                .Include(e => e.CreatingUser)
                .Include(e => e.Tags)
                .Include(e => e.PostCategory)
                .FirstOrDefault(e => e.Id == id);
        }

        //need to fix
        public List<Post> TopHot()
        {
            return _db.Posts
                .OrderByDescending(e => e.Id)
                .Skip(0)
                .Take(6)
                .ToList();
        }

        public List<Post> Recent(int page = 1, int pageSize = 6)
        {
            List<Post> posts;
            var count = _db.Posts
                .OrderByDescending(e => e.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Count();

            if (count < pageSize)
                posts = _db.Posts
                .OrderByDescending(e => e.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(count)
                .ToList();

            posts = _db.Posts
                .OrderByDescending(e => e.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return posts;
        }

        public bool IsAnyLeft(int page, int pageSize)
        {
            var count = _db.Posts
               .OrderByDescending(e => e.CreatedAt)
               .Skip((page - 1) * pageSize)
               .Count();
            if (count <= pageSize)
                return false;
            return true;
        }

        public bool IsAnyLeftInCategory(int categoryId, int page, int pageSize)
        {
            var count = _db.Posts
                .Where(e => e.CategoryId == categoryId)
               .OrderByDescending(e => e.CreatedAt)
               .Skip((page - 1) * pageSize)
               .Count();
            if (count <= pageSize)
                return false;
            return true;
        }

        public bool IsAnyLeftInSearch(string searchString, int page, int pageSize)
        {
            var searchTerms = StringHelper.FilterWhiteSpaces(searchString).Trim().Split(' ');

            var query = _db.Posts.AsQueryable();

            //eager loading
            query = query.Include(e => e.CreatingUser);
            query = query.Include(e => e.UpdatingUser);
            query = query.Include(e => e.PostCategory);
            query = query.Include(e => e.Tags);

            //exactly match
            var predicate = PredicateBuilder.New<Post>();
            predicate = predicate.Or(e => e.Title.Contains(searchString));
            predicate = predicate.Or(e => e.MetaTitle.Contains(searchString));
            predicate = predicate.Or(e => e.Summary.Contains(searchString));
            predicate = predicate.Or(e => e.Content.Contains(searchString));
            predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(searchString));
            predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(searchString));
            predicate = predicate.Or(e => e.References.Contains(searchString));
            predicate = predicate.Or(e => e.PostCategory.Name.Contains(searchString));
            predicate = predicate.Or(e => e.Tags.Select(t => t.Id).Contains(searchString));
            predicate = predicate.Or(e => e.Tags.Select(t => t.Name).Contains(searchString));

            //partial match
            foreach (var item in searchTerms)
            {
                if (item.Length != 1)
                {
                    predicate = predicate.Or(e => e.Title.Contains(item));
                    predicate = predicate.Or(e => e.Summary.Contains(item));
                    predicate = predicate.Or(e => e.Content.Contains(item));
                    predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(item));
                    predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(item));
                    predicate = predicate.Or(e => e.References.Contains(item));
                    predicate = predicate.Or(e => e.PostCategory.Name.Contains(item));
                    predicate = predicate.Or(e => e.MetaTitle.Contains(item));
                    predicate = predicate.Or(e => e.Tags.Any(t => t.Name.Contains(item)));
                    predicate = predicate.Or(e => e.Tags.Any(t => t.Id.Contains(item)));
                }
            }

            var count = query
                .Where(predicate)
                 .Count();

            if (count <= pageSize)
                return false;
            return true;
        }

        public bool IncreaseViewCount(long id)
        {
            bool isSuccess = true;
            try
            {
                var post = _db.Posts.Find(id);
                post.ViewCount = post.ViewCount + 1;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public List<Post> MostViews()
        {
            return _db.Posts
                .OrderByDescending(e => e.CreatedAt)
                .OrderByDescending(e => e.ViewCount)
                .Skip(0)
                .Take(5)
                .ToList();
        }

        public List<Post> MostComments()
        {
            return null;
        }

        public List<Post> RelatedPosts(int categoryId, long id)
        {
            return _db.Posts
                .Where(e => e.CategoryId == categoryId && e.Id != id)
                .OrderByDescending(e => e.CreatedAt)
                .Skip(0)
                .Take(4)
                .ToList();
        }

        public List<Post> GetByCategory(int categoryId, int page, int pageSize)
        {
            return _db.Posts
                .Where(e => e.CategoryId == categoryId)
                .OrderByDescending(e => e.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public List<Post> GetAll(int page, int pageSize)
        {
            return _db.Posts
                .Include(e => e.CreatingUser)
                .Include(e => e.PostCategory)
                .Include(e => e.Tags)
                .ToList();
        }

        public List<Post> Search(string searchString)
        {
            var searchTerms = StringHelper.FilterWhiteSpaces(searchString).Trim().Split(' ');

            var query = _db.Posts.AsQueryable();

            //eager loading
            query = query.Include(e => e.CreatingUser);
            query = query.Include(e => e.UpdatingUser);
            query = query.Include(e => e.PostCategory);
            query = query.Include(e => e.Tags);

            //exactly match
            var predicate = PredicateBuilder.New<Post>();
            predicate = predicate.Or(e => e.Title.Contains(searchString));
            predicate = predicate.Or(e => e.MetaTitle.Contains(searchString));
            predicate = predicate.Or(e => e.Summary.Contains(searchString));
            predicate = predicate.Or(e => e.Content.Contains(searchString));
            predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(searchString));
            predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(searchString));
            predicate = predicate.Or(e => e.References.Contains(searchString));
            predicate = predicate.Or(e => e.PostCategory.Name.Contains(searchString));
            predicate = predicate.Or(e => e.Tags.Select(t => t.Id).Contains(searchString));
            predicate = predicate.Or(e => e.Tags.Select(t => t.Name).Contains(searchString));

            //partial match
            foreach (var item in searchTerms)
            {
                if (item.Length != 1)
                {
                    predicate = predicate.Or(e => e.Title.Contains(item));
                    predicate = predicate.Or(e => e.Summary.Contains(item));
                    predicate = predicate.Or(e => e.Content.Contains(item));
                    predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(item));
                    predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(item));
                    predicate = predicate.Or(e => e.References.Contains(item));
                    predicate = predicate.Or(e => e.PostCategory.Name.Contains(item));
                    predicate = predicate.Or(e => e.MetaTitle.Contains(item));
                    predicate = predicate.Or(e => e.Tags.Any(t => t.Name.Contains(item)));
                    predicate = predicate.Or(e => e.Tags.Any(t => t.Id.Contains(item)));
                }
            }

            return query
                .Where(predicate)
                 .ToList();
        }

        public List<Post> SearchByCategory(string searchString, int categoryId, int page, int pageSize)
        {
            var searchTerms = StringHelper.FilterWhiteSpaces(searchString).Trim().Split(' ');

            var query = _db.Posts.AsQueryable();

            //eager loading
            query = query.Include(e => e.CreatingUser);
            query = query.Include(e => e.UpdatingUser);
            query = query.Include(e => e.PostCategory);
            query = query.Include(e => e.Tags);

            //exactly match
            var predicate = PredicateBuilder.New<Post>();
            predicate = predicate.Or(e => e.Title.Contains(searchString));
            predicate = predicate.Or(e => e.MetaTitle.Contains(searchString));
            predicate = predicate.Or(e => e.Summary.Contains(searchString));
            predicate = predicate.Or(e => e.Content.Contains(searchString));
            predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(searchString));
            predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(searchString));
            predicate = predicate.Or(e => e.References.Contains(searchString));
            predicate = predicate.Or(e => e.Tags.Select(t => t.Id).Contains(searchString));
            predicate = predicate.Or(e => e.Tags.Select(t => t.Name).Contains(searchString));

            //partial match
            foreach (var item in searchTerms)
            {
                if (item.Length != 1)
                {
                    predicate = predicate.Or(e => e.Title.Contains(item));
                    predicate = predicate.Or(e => e.Summary.Contains(item));
                    predicate = predicate.Or(e => e.Content.Contains(item));
                    predicate = predicate.Or(e => e.CreatingUser.FullName.Contains(item));
                    predicate = predicate.Or(e => e.UpdatingUser.FullName.Contains(item));
                    predicate = predicate.Or(e => e.References.Contains(item));
                    predicate = predicate.Or(e => e.MetaTitle.Contains(item));
                    predicate = predicate.Or(e => e.Tags.Any(t => t.Name.Contains(item)));
                    predicate = predicate.Or(e => e.Tags.Any(t => t.Id.Contains(item)));
                }
            }

            return query
                .Where(e => e.CategoryId == categoryId)
                .Where(predicate)
                .OrderByDescending(e => e.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                 .ToList();
        }

    }
}