using AutoMapper;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VNScience.Areas.Admin.DataAccess;
using VNScience.Common;
using VNScience.Models;
using VNScience.Models.Core;
using VNScience.ViewModels;

namespace VNScience.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        PostDAO postDAO;
        TagDAO tagDAO;
        public PostController()
        {
            postDAO = new PostDAO(db);
            tagDAO = new TagDAO(db);
        }

        // GET: Admin/Post
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Index(string searchString = null, int page = 1, int pageSize = 10)
        {
            List<Post> posts = new List<Post>();
            var model = new List<PostViewModel>();

            //search for post in db
            if (string.IsNullOrEmpty(searchString))
            {
                posts = postDAO.GetAllWithUserAndCategory();
                foreach (var item in posts)
                {
                    model.Add(Mapper.Map<PostViewModel>(item));
                }
            }
            else
            {
                posts = postDAO.Search(searchString);
                //process for best visualize data
                var searchParts = searchString.Split(' ');
                foreach (var post in posts)
                {
                    var postToDisplay = Mapper.Map<PostViewModel>(post);

                    if ((postToDisplay.Title != null ? postToDisplay.Title.Contains(searchString) : false)
                    || (postToDisplay.Summary != null ? postToDisplay.Summary.Contains(searchString) : false)
                    || (postToDisplay.Content != null ? postToDisplay.Content.Contains(searchString) : false)
                    || postToDisplay.CreatingUser.FullName.Contains(searchString)
                    || (postToDisplay.UpdatingUser != null ? postToDisplay.UpdatingUser.FullName.Contains(searchString) : false)
                    || (postToDisplay.References != null ? postToDisplay.References.Contains(searchString) : false)
                    || postToDisplay.PostCategory.Name.Contains(searchString)
                    || (postToDisplay.Tags != null ? postToDisplay.Tags.Select(e => e.Id.Replace('-', ' ')).Contains(searchString) : false)
                    || (postToDisplay.Tags != null ? postToDisplay.Tags.Select(e => e.Name).Contains(searchString) : false)
                    )
                    {
                        postToDisplay.SearchMatchingType = SearchMatchingType.FullyMatch;
                    }
                    else if ((postToDisplay.Title != null ? postToDisplay.Title.Split(' ').Intersect(searchParts).Count() == searchParts.Length : false)
                    || (postToDisplay.Summary != null ? postToDisplay.Summary.Split(' ').Intersect(searchParts).Count() == searchParts.Length : false)
                    || (postToDisplay.Content != null ? postToDisplay.Content.Split(' ').Intersect(searchParts).Count() == searchParts.Length : false)
                    || postToDisplay.CreatingUser.FullName.Split(' ').Intersect(searchParts).Count() == searchParts.Length
                    || (postToDisplay.UpdatingUser != null ? postToDisplay.UpdatingUser.FullName.Split(' ').Intersect(searchParts).Count() == searchParts.Length : false)
                    || (postToDisplay.References != null ? postToDisplay.References.Split(' ').Intersect(searchParts).Count() == searchParts.Length : false)
                    || postToDisplay.PostCategory.Name.Split(' ').Intersect(searchParts).Count() == searchParts.Length
                    || (postToDisplay.Tags != null ? postToDisplay.Tags.Any(e => e.Id.Split('-').Intersect(searchParts).Count() == searchParts.Length) : false)
                    || (postToDisplay.Tags != null ? postToDisplay.Tags.Any(e => e.Name.Split(' ').Intersect(searchParts).Count() == searchParts.Length) : false))
                    {
                        postToDisplay.SearchMatchingType = SearchMatchingType.FullyMatchButScrambled;
                    }
                    else
                    {
                        postToDisplay.SearchMatchingType = SearchMatchingType.PartialMatch;
                    }

                    model.Add(postToDisplay);
                }

                model = model.OrderBy(e => e.SearchMatchingType).ToList();
            }

            ViewBag.CurrentFilter = searchString;
            return View(model.ToPagedList(page, pageSize));
        }

        [HttpGet]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Create()
        {
            var postCategories = db.PostCategories.ToList();
            //postcategories selectlist
            var postCategoriesSelectList = new MySelectList()
            {
                FormElementName = "CategoryId"
            };
            foreach (var item in postCategories)
            {
                postCategoriesSelectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            var tags = tagDAO.GetAll();
            //tags selectlist
            var tagSelectList = new MySelectList()
            {
                FormElementName = "TagIds"
            };
            foreach (var item in tags)
            {
                tagSelectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            ViewBag.PostCategories = postCategoriesSelectList;
            ViewBag.Tags = tagSelectList;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Create(PostViewModel model)
        {
            bool isSuccess = true;
            if (!ModelState.IsValid)
                return View();

            //upload file
            var uploadResult = UploadFile(Common.Constants.AdminImagesUrl);
            model.CoverImage = uploadResult.Length == 0 ? "" : uploadResult[0];

            model.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? StringHelper.ToUnsignString(model.Title) : model.MetaTitle;
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = User.Identity.GetUserId();
            model.IsApproved = false;
            model.IsLock = false;
            model.ViewCount = 0;
            model.IsRequestedDelete = false;

            //insert post
            var newPost = Mapper.Map<Post>(model);
            isSuccess = postDAO.Insert(newPost) ? isSuccess : false;

            //insert tags for new post
            var moreTags = string.IsNullOrEmpty(model.MoreTags) ? new string[0] : model.MoreTags.Split(',');
            var tagIds = model.TagIds == null ? new string[0] : model.TagIds;
            var allTags = moreTags.Concat(tagIds).ToArray();

            isSuccess = postDAO.InsertTags(newPost, tagIds, moreTags) ? isSuccess : false;


            if (!isSuccess)
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);
            else
                Notification.Success("Xử lý bài viết hoàn tất", Session);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Preview(Post post)
        {
            return View(post);
        }
        
        [HttpGet]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult Edit(int id)
        {
            var editedPost = postDAO.GetWithTags(id);
            if (!postDAO.IsEditing(editedPost))
            {
                //mark as editing
                postDAO.MarkAsEditing(id);
            }
            else
            {
                Notification.Error("Bài viết đang được chỉnh sửa bởi người khác, vui lòng thử lại sau", Session);
                return RedirectToAction("Index");
            }

            PostViewModel model = Mapper.Map<PostViewModel>(editedPost);

            var postCategories = db.PostCategories.ToList();
            //postcategories selectlist
            var postCategoriesSelectList = new MySelectList()
            {
                FormElementName = "CategoryId"
            };
            foreach (var item in postCategories)
            {
                //selectedItems
                if (editedPost.CategoryId == item.Id)
                    postCategoriesSelectList.SelectedItems.Add(item.Id.ToString());

                //Items
                postCategoriesSelectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            var tags = tagDAO.GetAll();
            //tags selectlist
            var tagSelectList = new MySelectList()
            {
                FormElementName = "TagIds"
            };
            foreach (var item in tags)
            {
                //selectedItems
                if (editedPost.Tags.Select(e => e.Id).Contains(item.Id))
                {
                    tagSelectList.SelectedItems.Add(item.Id.ToString());
                }

                //Items
                tagSelectList.Items.Add(new MySelectListItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name
                });
            }

            ViewBag.PostCategories = postCategoriesSelectList;
            ViewBag.Tags = tagSelectList;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.PostMod)]
        [ValidateInput(false)]
        public ActionResult Edit(PostViewModel model)
        {
            bool isSuccess = true;
            if (!ModelState.IsValid)
                return View();

            //map model to editedPost
            Post editedPost = Mapper.Map<Post>(model);

            //update field
            editedPost.MetaTitle = string.IsNullOrEmpty(model.MetaTitle) ? StringHelper.ToUnsignString(editedPost.Title) : model.MetaTitle;
            editedPost.UpdatedAt = DateTime.Now;
            editedPost.UpdatedBy = User.Identity.GetUserId();
            editedPost.IsLock = false;
            ////upload file
            if (Request.Files[0].ContentLength > 0)
            {
                var uploadResult = UploadFile(Common.Constants.AdminImagesUrl);
                editedPost.CoverImage = uploadResult.Length == 0 ? "" : uploadResult[0];
            }
            else
            {
                editedPost.CoverImage = editedPost.CoverImage;
            }

            //save editedPost
            isSuccess = postDAO.Update(editedPost) ? isSuccess : false;

            //insert tags for new post
            var moreTags = string.IsNullOrEmpty(model.MoreTags) ? new string[0] : model.MoreTags.Split(',');
            var tagIds = model.TagIds == null ? new string[0] : model.TagIds;
            var allTags = moreTags.Concat(tagIds).ToArray();

            isSuccess = postDAO.InsertTags(editedPost, tagIds, moreTags) ? isSuccess : false;


            if (!isSuccess)
                Notification.Error("Có lỗi xảy ra, vui lòng thử lại sau", Session);
            else
                Notification.Success("Xử lý bài viết hoàn tất", Session);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin + "," + RoleName.PostMod)]
        public ActionResult Details(int id)
        {
            var model = postDAO.GetWithUserAndCategoryAndTags(id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.PostMod)]
        public JsonResult Delete(int id)
        {
            bool isSuccess = true;
            var message = "";
            DeleteType deleteType;
            var postToDelete = postDAO.Get(id);

            //if post was approved, mod cannot immediately destroy it
            if (!postToDelete.IsApproved.Value)
            {
                isSuccess = postDAO.Destroy(id);
                message = "Đã xóa thành công bài viết";
                deleteType = DeleteType.Destroy;
            }
            else
            {
                isSuccess = postDAO.MarkAsDelete(id);
                message = "Đã gửi yêu cầu xóa đến quản trị viên";
                deleteType = DeleteType.WaitingForAccept;
            }

            return Json(new { status = isSuccess ? 200 : 500, message = message, deleteType = deleteType }, JsonRequestBehavior.AllowGet);
        }

        #region For Admin
        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public JsonResult Approve(int id)
        {
            var message = "";
            bool isSuccess = postDAO.Approve(id);
            if (isSuccess)
                message = "Đã duyệt! Bài viết sẽ hiển thị trên trang chủ";
            else
                message = "Có lỗi xảy ra, vui lòng thử lại sau";

            return Json(new { status = isSuccess ? 200 : 500, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public JsonResult UnApprove(int id)
        {
            var message = "";
            bool isSuccess = postDAO.UnApprove(id);
            if (isSuccess)
                message = "Đã hủy duyệt! Bài viết sẽ xóa khỏi trang chủ";
            else
                message = "Có lỗi xảy ra, vui lòng thử lại sau";
            return Json(new { status = isSuccess ? 200 : 500, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult RequestsToApprove()
        {
            var notApprovedPosts = postDAO.GetManyNotApproved();
            return View(notApprovedPosts);
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult RequestsToDelete()
        {
            var deletedRequests = postDAO.GetManyDeleteRequested();
            return View(deletedRequests);
        }

        [HttpGet]
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult ApprovedPosts()
        {
            var approvedPosts = postDAO.GetManyApproved();
            return View(approvedPosts);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public JsonResult PermanentDelete(int id)
        {
            var message = "";
            bool isSuccess = postDAO.Destroy(id);
            if (isSuccess)
                message = "Đã xóa thành công bài viết";
            else
                message = "Có lỗi xảy ra, vui lòng thử lại sau";
            return Json(new { status = isSuccess ? 200 : 500, message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.Admin)]
        public JsonResult RefusePermanentDelete(int id)
        {
            var message = "";
            bool isSuccess = postDAO.UnRequestDelete(id);
            if (isSuccess)
                message = "Đã hủy thành công yêu cầu xóa";
            else
                message = "Có lỗi xảy ra, vui lòng thử lại sau";
            return Json(new { status = isSuccess ? 200 : 500, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion





        #region For Mod
        [HttpGet]
        [Authorize(Roles = RoleName.PostMod)]
        public ActionResult ModRequestsToApprove()
        {
            var modApprovedRequests = postDAO.GetManyNotApprovedByUser(User.Identity.GetUserId());
            return View(modApprovedRequests);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.PostMod)]
        public JsonResult MarkAsCanEdit(int id)
        {
            postDAO.MarkAsCanEdit(id);

            return Json(new { status = 200 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ConfirmCreate(long id)
        {
            
            return RedirectToAction("Index");
        }

        #endregion
    }
}