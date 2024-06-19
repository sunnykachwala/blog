namespace Np.ViewModel
{
    public class AppSettingDto
    {      
        public Guid Id { get; set; }
        public string ApplicationName { get; set; }       
        public string Logo { get; set; }     
        public string AppIcon { get; set; }
        public int DefaultCustomer { get; set; }
        public int SaleAccount { get; set; }
        public int PurchaseAccount { get; set; }
        public int PayrollAccount { get; set; }      
        public string Copyright { get; set; }      
        public string CompanyName { get; set; }       
        public string CompanyEmail { get; set; }     
        public string CompanyPhone { get; set; }        
        public string CompanyAddress { get; set; }       
        public string CompanyCity { get; set; }       
        public string CompanyState { get; set; }      
        public string CompanyPostalCode { get; set; }       
        public string CompanyCountry { get; set; }      
        public string CompanyTaxNumber { get; set; }        
        public string DefaultTimezone { get; set; }       
        public string DefaultLanguage { get; set; }       
        public string DefaultCurrency { get; set; }
        public string MailProtocol { get; set; }     
        public string MailEncryption { get; set; }        
        public string MailHost { get; set; }
        public int MailPort { get; set; }       
        public string MailUserName { get; set; }       
        public string MailPassword { get; set; }
        public bool SendInvoice { get; set; }      
        public string InvoiceTemplate { get; set; }     
        public string ThemeLayout { get; set; }     
        public string ThemeColor { get; set; }       
        public string ThemeAppBar { get; set; }        
        public string ThemeSideBar { get; set; }
        public int AwardedPointsPerSpent { get; set; }
        public decimal RewardPointsWorth { get; set; }
    }
}
