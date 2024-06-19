namespace Np.ViewModel
{
    using System.ComponentModel.DataAnnotations;
    public class CreateUserDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;
        public Guid UserRoleGuid { get; set; }

        public bool IsActive { get; set; }
        public bool TwoFactor { get; set; } = true;
        public Guid OrganisationGuid { get; set; }

    }
}
