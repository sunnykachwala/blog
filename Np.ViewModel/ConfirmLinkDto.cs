namespace Np.ViewModel
{
    using System.ComponentModel.DataAnnotations;
    public class ConfirmLinkDto
    {
        [Required]
        public Guid LoginGuid { get; set; }

        [Required]
        public string ConfirmationCode { get; set; }

        public bool IsFirstLogin { get; set; }

        public string ResetType { get; set; } = "QR";
    }
}
