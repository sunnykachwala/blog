namespace Np.ViewModel
{
    public class UserFilterDto : FilterDto
    {
        public Guid? UserRoleGuid { get; set; }
        public Guid? OrganisationGuid { get; set; }
    }
}
