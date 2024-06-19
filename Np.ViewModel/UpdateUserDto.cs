namespace Np.ViewModel
{
    public class UpdateUserDto
    {
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public Guid UserRoleGuid { get; set; }
        public Guid OrganisationGuid { get; set; }
        public bool TwofactorEnabled { get; set; }
    }
}
