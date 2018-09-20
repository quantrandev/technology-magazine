namespace VNScience.Models.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Tên đầy đủ")]
        public string FullName { get; set; }

        [StringLength(500)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        [StringLength(2000)]
        [Display(Name = "Nội dung")]
        public string Message { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime? CreatedAt { get; set; }

        [Display(Name = "Đã xem")]
        public bool IsSeen { get; set; }
    }
}
