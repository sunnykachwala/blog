namespace Np.ViewModel
{
    using System.ComponentModel.DataAnnotations;
    public class LoginResetHistoryDto
    {
        [Required]
        public Guid LoginGuid { get; set; }

        [Required]
        public string ResetType { get; set; }

        [Required]
        public DateTime ResetExpiryTime { get; set; }

        [Required]
        public byte[] Salt { get; set; }

        [Required]
        public string HashedConfirmationCode { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
