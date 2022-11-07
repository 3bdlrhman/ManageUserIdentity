using System.ComponentModel.DataAnnotations;

namespace ManageUserIdentity.ViewModels
{
    public class RoleViewModel
    {
        [Required, StringLength(250)]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
