using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabPro.Web.Models
{
    public class Note : DatabaseEntity<int>
    {

    }

    public enum NoteType 
    { 
        MainPage,
        ProfessionalOpinion
    }

}
