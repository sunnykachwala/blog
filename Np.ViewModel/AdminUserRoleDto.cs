namespace Np.ViewModel
{
    public class AdminUserRoleDto
    {
        public Guid UserRoleGuid { get; set; }
        public string Name { get; set; }
        public bool IsDefaultGroup { get; set; } = false;
    }
}
