﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VNScience.Common;
using VNScience.DataAccess;
using VNScience.Models;
using VNScience.Models.Core;
using VNScience.ViewModels;

namespace VNScience.Controllers
{
    public class PostController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ClientPostDAO postDAO;
        ClientPostCategoryDAO postCategoryDAO;

        public PostController()
        {
            postDAO = new ClientPostDAO(db);
            postCategoryDAO = new ClientPostCategoryDAO(db);
        }

        public ActionResult Detail(long id, string searchString = null)
        {
            var post = postDAO.Get(id);
            var model = Mapper.Map<PostViewModel>(post);

            var relatedPosts = new List<PostViewModel>();
            var relatedPostsInDb = postDAO.RelatedPosts(post.CategoryId.Value, id);
            foreach (var item in relatedPostsInDb)
            {
                relatedPosts.Add(Mapper.Map<PostViewModel>(item));
            }
            ViewBag.RelatedPosts = relatedPosts;

            if (searchString != null)
                ViewBag.SearchString = searchString;
            return View(model);
        }

        public ActionResult Index(int categoryId, int page = 1, int pageSize = 8)
        {
            List<Post> posts = new List<Post>();
            var model = new List<PostViewModel>();

            posts = postDAO.GetByCategory(categoryId, page, pageSize);
            foreach (var item in posts)
            {
                model.Add(Mapper.Map<PostViewModel>(item));
            }

            ViewBag.Category = postCategoryDAO.Get(categoryId);
            return View(model);
        }

        public ActionResult Search(string searchString, int page = 1, int pageSize = 8)
        {
            List<Post> posts = new List<Post>();
            var model = new List<PostViewModel>();

            //search for post in db
            if (string.IsNullOrEmpty(searchString))
            {
                posts = postDAO.GetAll(page, pageSize);
                foreach (var item in posts)
                {
                    model.Add(Mapper.Map<PostViewModel>(item));
                }
            }
            else
            {
                model = ProcessPost(postDAO.Search(searchString), searchString)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            ViewBag.IsAnyLeft = IsAnyLeftInSearch(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Tags(string tag)
        {
            var model = new List<PostViewModel>();
            var postsByTag = postDAO.GetByTag(tag);

            foreach (var item in postsByTag)
            {
                model.Add(Mapper.Map<PostViewModel>(item));
            }

            ViewBag.Tag = db.Tags.Find(tag).Name;
            return View(model);
        }

        //helpers
        [HttpGet]
        public JsonResult CategoryPaging(int categoryId, int page, int pageSize)
        {
            var posts = postDAO.GetByCategory(categoryId, page, pageSize).Select(e => new
            {
                Id = e.Id,
                Title = e.Title,
                MetaTitle = e.MetaTitle,
                Author = e.CreatingUser.FullName,
                ViewCount = e.ViewCount,
                Time = DateTimeHelper.FormatDate(e.CreatedAt.Value),
                CoverImage = e.CoverImage
            });

            bool isAnyLeft = postDAO.IsAnyLeftInCategory(categoryId, page, pageSize);

            return Json(new { status = 200, data = posts, isAnyLeft = isAnyLeft }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchPaging(string searchString, int page, int pageSize)
        {
            var posts = postDAO.Search(searchString);

            var model = ProcessPost(posts, searchString)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new
                {
                    Id = e.Id,
                    Title = e.Title,
                    MetaTitle = e.MetaTitle,
                    Author = e.CreatingUser.FullName,
                    ViewCount = e.ViewCount,
                    Time = DateTimeHelper.FormatDate(e.CreatedAt.Value),
                    CoverImage = e.CoverImage,
                    Content = e.Content
                });

            bool isAnyLeft = IsAnyLeftInSearch(searchString, page, pageSize);

            return Json(new { status = 200, data = model, isAnyLeft = isAnyLeft }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IncreaseViewCount(long id)
        {
            bool isSuccess = postDAO.IncreaseViewCount(id);

            return Json(new { status = 200 }, JsonRequestBehavior.AllowGet);
        }

        public bool IsAnyLeftInSearch(string searchString, int page, int pageSize)
        {
            var count = postDAO.Search(searchString)
                .Skip((page - 1) * pageSize)
                .Count();

            if (count <= pageSize)
                return false;
            return true;
        }

        public List<PostViewModel> ProcessPost(List<Post> posts, string searchString)
        {
            searchString = searchString.ToLower();
            List<PostViewModel> model = new List<PostViewModel>();

            //process for best visualize data
            var searchParts = searchString.Split(' ');
            foreach (var post in posts)
            {
                var postToDisplay = Mapper.Map<PostViewModel>(post);

                if ((postToDisplay.Title != null ? postToDisplay.Title.ToLower().Contains(searchString) : false))
                {
                    postToDisplay.SearchMatchingType = SearchMatchingType.FullyMatchTitle;
                }
                else if ((postToDisplay.Summary != null ? postToDisplay.Summary.ToLower().Contains(searchString) : false)
                || (postToDisplay.Content != null ? postToDisplay.Content.ToLower().Contains(searchString) : false)
                || postToDisplay.CreatingUser.FullName.ToLower().Contains(searchString)
                || (postToDisplay.UpdatingUser != null ? postToDisplay.UpdatingUser.FullName.ToLower().Contains(searchString) : false)
                || (postToDisplay.References != null ? postToDisplay.References.ToLower().Contains(searchString) : false)
                || postToDisplay.PostCategory.Name.ToLower().Contains(searchString)
                || (postToDisplay.Tags != null ? postToDisplay.Tags.Select(e => e.Id.Replace('-', ' ')).Contains(searchString) : false)
                || (postToDisplay.Tags != null ? postToDisplay.Tags.Select(e => e.Name).Contains(searchString) : false)
                )
                {
                    postToDisplay.SearchMatchingType = SearchMatchingType.FullyMatchOther;
                }
                else if ((postToDisplay.Title != null ? postToDisplay.Title.ToLower().Split(' ').Intersect(searchParts).Count() == searchParts.Length : false))
                {
                    postToDisplay.SearchMatchingType = SearchMatchingType.FullyMatchTitleButScrambled;
                }
                else if ((postToDisplay.Summary != null ? postToDisplay.Summary.ToLower().Split(' ').Intersect(searchParts).Count() == searchParts.Length : false)
                || (postToDisplay.Content != null ? postToDisplay.Content.ToLower().Split(' ').Intersect(searchParts).Count() == searchParts.Length : false)
                || postToDisplay.CreatingUser.FullName.ToLower().Split(' ').Intersect(searchParts).Count() == searchParts.Length
                || (postToDisplay.UpdatingUser != null ? postToDisplay.UpdatingUser.FullName.ToLower().Split(' ').Intersect(searchParts).Count() == searchParts.Length : false)
                || (postToDisplay.References != null ? postToDisplay.References.ToLower().Split(' ').Intersect(searchParts).Count() == searchParts.Length : false)
                || (postToDisplay.Tags != null ? postToDisplay.Tags.Any(e => e.Id.Split('-').Intersect(searchParts).Count() == searchParts.Length) : false)
                || (postToDisplay.Tags != null ? postToDisplay.Tags.Any(e => e.Name.Split(' ').Intersect(searchParts).Count() == searchParts.Length) : false)
                || postToDisplay.PostCategory.Name.ToLower().Split(' ').Intersect(searchParts).Count() == searchParts.Length)
                {
                    postToDisplay.SearchMatchingType = SearchMatchingType.FullyMatchOtherButScrambled;
                }
                else
                {
                    postToDisplay.SearchMatchingType = SearchMatchingType.PartialMatch;
                }

                model.Add(postToDisplay);
            }

            model = model.OrderBy(e => e.SearchMatchingType)
                .ToList();


            return model;
        }
    }
}