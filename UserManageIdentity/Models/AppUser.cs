using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;

namespace ManageUserIdentity.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePic { get; set; }
    }
}
