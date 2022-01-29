using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace LabPro.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        [NotMapped]
        public IEnumerable<string> RoleNames { get; set; }

        [IgnoreDataMember]
        public override string PasswordHash { get; set; }

        [IgnoreDataMember, NotMapped]
        public string Password { get; set; }

        [IgnoreDataMember, NotMapped]
        public string ConfirmPassword { get; set; }

        [IgnoreDataMember, NotMapped]
        public string Name
        {
            get
            {
                return UserName;
            }
            set
            {
                UserName = value;
            }
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
    }
}
