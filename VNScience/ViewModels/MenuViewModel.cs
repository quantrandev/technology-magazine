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
        public string Title { get; set; }

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

        public byte? MenuTypeId { get; set; }

        public MenuType MenuType { get; set; }

        public virtual ApplicationUser CreatingUser { get; set; }
        public virtual ApplicationUser UpdatingUser { get; set; }

        public SearchMatchingType SearchMatchingType { get; set; }
    }
}