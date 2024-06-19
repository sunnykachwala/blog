namespace Np.ViewModel
{
    public class UserListingDto
    {
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string UserType { get; set; }
        public string RoleName { get; set; }

        public string OrganisationName { get; set; }
    }
}
