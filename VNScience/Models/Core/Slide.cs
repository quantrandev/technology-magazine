using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VNScience.Models.Core
{
    [Table("Slide")]
    public partial class Slide
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Link { get; set; }

        [StringLength(50)]
        public string Target { get; set; }

        public int? DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        [StringLength(128)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(128)]
        public string UpdatedBy { get; set; }

        public virtual ApplicationUser CreatingUser { get; set; }
        public virtual ApplicationUser UpdatingUser { get; set; }
    }
}