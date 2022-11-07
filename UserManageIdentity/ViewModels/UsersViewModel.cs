namespace ManageUserIdentity.ViewModels
{
    public class UsersViewModel
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        public List<string> UserRoles { get; set; }
    }
}
