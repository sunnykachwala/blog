namespace Np.ViewModel
{
    public class UserInfoWithRoleDto : UserInfoDto
    {
        public string RoleName { get; set; }
        public string OrganisationName { get; set; }
        public string DefaultHome { get; set; }
        public string Permissions { get; set; }
    }
}
