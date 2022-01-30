using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LabPro.Web.Models
{
    public partial class ApplicationUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }
}
