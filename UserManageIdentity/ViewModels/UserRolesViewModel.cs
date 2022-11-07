namespace ManageUserIdentity.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleModel> Roles { get; set; }
    }

    public class RoleModel
    {
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public bool IsRoleSelected { get; set; }

    }
}
