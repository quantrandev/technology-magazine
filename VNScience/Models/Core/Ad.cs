﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VNScience.Models.Core
{
    [Table("Ad")]
    public partial class Ad
    {
        public int Id { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Content { get; set; }

        [Display(Name = "Liên kết")]
        public string Link { get; set; }

        [StringLength(50)]
        public string Target { get; set; }

        [Display(Name = "Số lần click")]
        public long ClickCount { get; set; }

        [Display(Name = "Vị trí hiển thị")]
        public int Position { get; set; }

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

        public virtual ApplicationUser CreatingUser { get; set; }
        public virtual ApplicationUser UpdatingUser { get; set; }
    }
}