namespace Np.ViewModel
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class LoginDto
    {
        [Required]     
        public string OrganisationName { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}