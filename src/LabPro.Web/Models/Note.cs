using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabPro.Web.Models
{
    public class Note : DatabaseEntity<int>
    {
        [Required]
        public NoteCategory Category { get; set; }

        [Required]
        [StringLength(100)]
        public string SubCategory { get; set; }
        public string Text { get; set; }
    }

    public enum NoteCategory
    { 
        MainPage,
        ProfessionalOpinion
    }

}
