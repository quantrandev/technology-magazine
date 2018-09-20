namespace VNScience.Models.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PostCategory")]
    public partial class PostCategory
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; }

        [StringLength(100)]
        [Display(Name = "Tiêu đề SEO")]
        public string MetaTitle { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool? IsActive { get; set; }

        [Display(Name = "Hiển thị trên trang")]
        public bool? IsDisplayed { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Đang được yêu cầu xóa")]
        public bool? IsRequestedDelete { get; set; }

        [StringLength(128)]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày sửa")]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(128)]
        [Display(Name = "Người sửa")]
        public string UpdatedBy { get; set; }

        public virtual ApplicationUser UpdatingUser { get; set; }
        public virtual ApplicationUser CreatingUser { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
