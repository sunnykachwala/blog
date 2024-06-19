namespace Np.ViewModel
{
    public class LoginHistoryDto
    {
        public Guid LoginGuid { get; set; }
        public bool IsActive { get; set; }
        public string ResetType { get; set; }
        public DateTime ResetExpiryTime { get; set; }
        public byte[] Salt { get; set; }
        public string HashedConfirmationCode { get; set; }
        public string UserEmail { get; set; }
        public byte[] OriginalSalt { get; set; }
    }
}
