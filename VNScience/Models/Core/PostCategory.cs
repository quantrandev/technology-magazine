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
        public string Name { get; set; }

        [StringLength(100)]
        public string MetaTitle { get; set; }

        public string Description { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDisplayed { get; set; }

        public int? DisplayOrder { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool? IsRequestedDelete { get; set; }

        [StringLength(128)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(128)]
        public string UpdatedBy { get; set; }

        public virtual ApplicationUser UpdatingUser { get; set; }
        public virtual ApplicationUser CreatingUser { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
