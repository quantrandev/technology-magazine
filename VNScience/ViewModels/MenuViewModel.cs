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
    public class MenuViewModel
    {
        public int Id { get; set; }

        [StringLength(100)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [Display(Name = "Đường dẫn")]
        public string Link { get; set; }

        [StringLength(50)]
        public string Target { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { get; set; }

        [StringLength(128)]
        [Display(Name = "Người tạo")]
        public string CreatedBy { get; set; }

        [Display(Name = "Ngày sửa")]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(128)]
        [Display(Name = "Người sửa")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Loại Menu")]
        public byte? MenuTypeId { get; set; }

        [Display(Name = "Loại Menu")]
        public MenuType MenuType { get; set; }

        public virtual ApplicationUser CreatingUser { get; set; }
        public virtual ApplicationUser UpdatingUser { get; set; }

        public SearchMatchingType SearchMatchingType { get; set; }
    }
}