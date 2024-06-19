namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class AppSetting : IDefault, IAudited
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ApplicationName { get; set; }

        [Required]
        [StringLength(300)]
        public string Logo { get; set; }

        [Required]
        [StringLength(300)]
        public string AppIcon { get; set; }

        public int DefaultCustomer { get; set; }

        public int SaleAccount { get; set; }

        public int PurchaseAccount { get; set; }

        public int PayrollAccount { get; set; }

        [Required]
        [StringLength(100)]
        public string Copyright { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100)]
        public string CompanyEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyPhone { get; set; }

        [Required]
        [StringLength(250)]
        public string CompanyAddress { get; set; }

        [StringLength(250)]
        public string CompanyCity { get; set; }

        [StringLength(250)]
        public string CompanyState { get; set; }

        [StringLength(250)]
        public string CompanyPostalCode { get; set; }

        [StringLength(250)]
        public string CompanyCountry { get; set; }

        [Required]
        [StringLength(50)]
        public string CompanyTaxNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string DefaultTimezone { get; set; }

        [Required]
        [StringLength(20)]
        public string DefaultLanguage { get; set; }

        [Required]
        [StringLength(10)]
        public string DefaultCurrency { get; set; }

        [Required]
        [StringLength(50)]
        public string MailProtocol { get; set; }

        [Required]
        [StringLength(50)]
        public string MailEncryption { get; set; }

        [Required]
        [StringLength(100)]
        public string MailHost { get; set; }

        public int MailPort { get; set; }

        [Required]
        [StringLength(100)]
        public string MailUserName { get; set; }

        [Required]
        [StringLength(100)]
        public string MailPassword { get; set; }

        public bool SendInvoice { get; set; }

        [Required]
        [StringLength(100)]
        public string InvoiceTemplate { get; set; }

        [Required]
        [StringLength(100)]
        public string ThemeLayout { get; set; }

        [Required]
        [StringLength(50)]
        public string ThemeColor { get; set; }

        [Required]
        [StringLength(50)]
        public string ThemeAppBar { get; set; }

        [Required]
        [StringLength(50)]
        public string ThemeSideBar { get; set; }

        public int AwardedPointsPerSpent { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal RewardPointsWorth { get; set; }

        #region Inherited
        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string SystemUser { get; set; } = string.Empty;
        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string AppName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; } = string.Empty;
        #endregion
    }
}
