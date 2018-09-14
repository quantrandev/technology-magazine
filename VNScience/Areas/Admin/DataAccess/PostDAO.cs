using AutoMapper;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using VNScience.Common;
using VNScience.Models;
using VNScience.Models.Core;
using VNScience.ViewModels;

namespace VNScience.Areas.Admin.DataAccess
{
    public class PostDAO
    {
        ApplicationDbContext _db;
        TagDAO tagDAO;
        public PostDAO(ApplicationDbContext db)
        {
            this._db = db;
            tagDAO = new TagDAO(db);
        }

        //GET
        public Post Get(long id)
        {
            return _db.Posts
                .Find(id);
        }

        public Post GetWithTags(int id)
        {
            return _db.Posts
                .Include(e => e.Tags)
                .FirstOrDefault(e => e.Id == id);
        }

        public Post GetWithUserAndCategory(int id)
        {
            return _db.Posts
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.PostCategory)
                .FirstOrDefault(e => e.Id == id);
        }

        public List<Post> GetAllWithUserAndCategory()
        {
            return _db.Posts
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.PostCategory)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        public Post GetWithUserAndCategoryAndTags(long id)
        {
            return _db.Posts
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.PostCategory)
                .Include(e => e.Tags)
                .FirstOrDefault(e => e.Id == id);
        }

        public List<Post> GetManyNotApproved()
        {
            return _db.Posts
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.PostCategory)
                .OrderByDescending(e => e.CreatedAt)
                .Where(e => !e.IsApproved.Value).ToList();
        }

        public List<Post> GetManyDeleteRequested()
        {
            return _db.Posts
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.PostCategory)
                .OrderByDescending(e => e.CreatedAt)
                .Where(e => e.IsRequestedDelete.Value).ToList();
        }

        public List<Post> GetManyNotApprovedByUser(string userId)
        {
            return _db.Posts
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.PostCategory)
                .Where(e => e.CreatingUser.Id == userId && !e.IsApproved.Value)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        public List<Post> GetManyApproved()
        {
            return _db.Posts
                .Include(e => e.CreatingUser)
                .Include(e => e.UpdatingUser)
                .Include(e => e.PostCategory)
                .Where(e => e.IsApproved.Value)
                .OrderByDescending(e => e.CreatedAt)
                .ToList();
        }

        public int GetRequestsToApproveCount()
        {
            return _db.Posts.Where(e => !e.IsApproved.Value).Count();
        }
        public int GetRequestsToDeleteCount()
        {
            return _db.Posts.Where(e => e.IsRequestedDelete.Value).Count();
        }
        //END GET


        #region search

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
                if(item.Length != 1)
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

            return query.Where(predicate)
                 .ToList();
        }

        #endregion

        #region Helpers
        public bool Insert(Post newPost)
        {
            bool isSuccess = true;
            try
            {
                _db.Posts.Add(newPost);
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Update(Post updatedPost)
        {
            bool isSuccess = true;
            //try
            //{
            _db.Entry(updatedPost).State = EntityState.Modified;
            _db.SaveChanges();
            //}
            //catch (Exception e) { isSuccess = false; };

            return isSuccess;
        }

        public bool Destroy(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.Posts.Remove(_db.Posts.Find(id));
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool MarkAsDelete(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.Posts.Find(id).IsRequestedDelete = true;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool UnRequestDelete(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.Posts.Find(id).IsRequestedDelete = false;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool Approve(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.Posts.Find(id).IsApproved = true;
                _db.SaveChanges();
            }
            catch (Exception e) { isSuccess = false; }

            return isSuccess;
        }

        public bool UnApprove(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.Posts.Find(id).IsApproved = false;
                _db.SaveChanges();
            }
            catch (Exception e) { isSuccess = false; }

            return isSuccess;
        }

        public bool InsertTags(Post post, string[] tagIds, string[] moreTags)
        {
            //tagIds = tags id from selectList
            //moreTags = tags name from input
            bool isSuccess = true;
            post.Tags = tagDAO.GetByPost(post.Id);

            List<Tag> addedTags = new List<Tag>();
            List<Tag> deletedTags = new List<Tag>();

            //add tags from tagIds
            foreach (var tagId in tagIds)
            {
                //if post dont have this tag
                if (!post.Tags.Select(e => e.Id).Contains(tagId))
                    addedTags.Add(tagDAO.Get(tagId));
            }
            //add tag from moretags
            foreach (var tagName in moreTags)
            {
                addedTags.Add(new Tag()
                {
                    Id = StringHelper.ToUnsignString(tagName),
                    Name = tagName
                });
            }

            //delete tags from post tags
            foreach (var tag in post.Tags)
            {
                if (!tagIds.Contains(tag.Id))
                    deletedTags.Add(tag);
            }


            // add and delete post tags
            foreach (var item in addedTags)
            {
                post.Tags.Add(item);
            }
            foreach (var item in deletedTags)
            {
                post.Tags.Remove(item);
            }

            //save post tags to db
            try
            {
                _db.Entry(post).State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }
            return isSuccess;
        }

        public bool MarkAsEditing(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.Posts.Find(id).IsLock = true;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public bool MarkAsCanEdit(int id)
        {
            bool isSuccess = true;
            try
            {
                _db.Posts.Find(id).IsLock = false;
                _db.SaveChanges();
            }
            catch (Exception e)
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        
        public bool IsEditing(Post post)
        {
            return post.IsLock;
        }
        #endregion
    }
}