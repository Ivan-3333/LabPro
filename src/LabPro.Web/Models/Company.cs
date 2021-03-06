using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabPro.Web.Models
{
    public class Company : DatabaseEntity<int>
    {        
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(150)]
        public string Address { get; set; }

        [StringLength(150)]
        public string City { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        public bool IsClient { get; set; }
        public bool IsImporter { get; set; }
        public bool IsTransporter { get; set; }

        public List<Location> Locations { get; set; }

        public List<ContactPerson> ContactPersons { get; set; }

    }
}
