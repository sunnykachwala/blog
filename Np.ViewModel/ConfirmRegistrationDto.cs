namespace Np.ViewModel
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class ConfirmRegistrationDto
    {
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        public string ConfirmationCode { get; set; }

        [Required]
        public Guid LoginGuid { get; set; }
    }
}
