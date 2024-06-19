namespace Np.ViewModel
{
    using System.ComponentModel.DataAnnotations;

    public class ConfirmLinkWithPasswordDto : ConfirmLinkDto
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
