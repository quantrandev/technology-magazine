using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VNScience.Models.Core
{
    [Table("ProductCategory")]
    public partial class ProductCategory
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public string CoverImage { get; set; }

        public int? DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        [StringLength(128)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(128)]
        public string UpdatedBy { get; set; }

        public int? ParentId { get; set; }

        public virtual ProductCategory Parent { get; set; }
        public virtual List<ProductCategory> Children { get; set; }

        public virtual ApplicationUser CreatingUser { get; set; }
        public virtual ApplicationUser UpdatingUser { get; set; }
    }
}