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
        public string Title { get; set; }

        [StringLength(500)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        [StringLength(500)]
        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        public string JobRequirements { get; set; }

        public string JobWelfare { get; set; }

        public string JobAdditionalInfo { get; set; }

        public int? Quantity { get; set; }

        [StringLength(500)]
        public string JobWorkPlace { get; set; }

        public DateTime? CreatedAt { get; set; }

        [StringLength(128)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(128)]
        public string UpdatedBy { get; set; }
    }
}
