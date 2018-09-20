namespace VNScience.Models.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recruitment")]
    public partial class Recruitment
    {
        public long Id { get; set; }


        [StringLength(500)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [StringLength(500)]
        [Display(Name = "Tiêu đề SEO")]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }

        [StringLength(500)]
        [Display(Name = "Vị trí tuyển")]
        public string JobTitle { get; set; }

        [Display(Name = "Mô tả công việc")]
        public string JobDescription { get; set; }

        [Display(Name = "Yêu cầu công việc")]
        public string JobRequirements { get; set; }

        [Display(Name = "Chế độ")]
        public string JobWelfare { get; set; }

        [Display(Name = "Thông tin khác")]
        public string JobAdditionalInfo { get; set; }

        [Display(Name = "Số lượng tuyển")]
        public int? Quantity { get; set; }

        [StringLength(500)]
        [Display(Name = "Nơi làm việc")]
        public string JobWorkPlace { get; set; }

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

        public ApplicationUser CreatingUser { get; set; }
        public ApplicationUser UpdatingUser { get; set; }
    }
}
