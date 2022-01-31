using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabPro.Web.Models
{
    public class Location : DatabaseEntity<int>
    {

        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
