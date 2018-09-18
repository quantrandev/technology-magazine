using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VNScience.Common;
using VNScience.Models;
using VNScience.Models.Core;

namespace VNScience.ViewModels
{
    public class PostViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Tiêu đề SEO")]
        public string MetaTitle { get; set; }

        [StringLength(128)]
        public string Author { get; set; }

        [StringLength(5000)]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Display(Name = "Ảnh bìa")]
        public string CoverImage { get; set; }

        [Display(Name = "Lượt xem")]
        public int? ViewCount { get; set; }

        [Display(Name = "Duyệt đăng")]
        public bool? IsApproved { get; set; }

        public bool? IsPreview { get; set; }

        [Display(Name = "Yêu cầu xóa")]
        public bool? IsRequestedDelete { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedAt { get; set; }

        [StringLength(128)]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày sửa")]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(128)]
        [Display(Name = "Người sửa")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Danh mục")]
        public int? CategoryId { get; set; }

        [Display(Name = "Danh mục")]
        public PostCategory PostCategory { get; set; }

        public bool IsLock { get; set; }

        [Display(Name = "Tham khảo")]
        public string References { get; set; }

        [Display(Name = "Người sửa")]
        public virtual ApplicationUser UpdatingUser { get; set; }

        [Display(Name = "Người tạo")]
        public virtual ApplicationUser CreatingUser { get; set; }

        [Display(Name = "Tag")]
        public virtual ICollection<Tag> Tags { get; set; }

        [Display(Name = "Chọn tag")]
        public string[] TagIds { get; set; }

        [Display(Name = "Thêm tag mới")]
        public string MoreTags { get; set; }

        public SearchMatchingType SearchMatchingType { get; set; }
    }
}