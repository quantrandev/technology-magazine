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
        public string FullName { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Message { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool IsSeen { get; set; }
    }
}
