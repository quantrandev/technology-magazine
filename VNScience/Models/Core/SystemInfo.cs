namespace VNScience.Models.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemInfo")]
    public partial class SystemInfo
    {
        [StringLength(50)]
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [StringLength(128)]
        public string UpdatedBy { get; set; }
    }
}
