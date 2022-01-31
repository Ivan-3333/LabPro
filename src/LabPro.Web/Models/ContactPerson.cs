using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabPro.Web.Models
{
    public class ContactPerson : DatabaseEntity<int>
    {
        [Required]
        [StringLength(150)]
        public string FistName { get; set; }

        [Required]
        [StringLength(150)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(150)]
        public string City { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
