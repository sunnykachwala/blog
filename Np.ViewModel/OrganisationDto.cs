namespace Np.ViewModel
{
    using System.ComponentModel.DataAnnotations;
    public class OrganisationDto
    {
        [Key]
        public Guid OrganisationGuid { get; set; }

        [Required]
        [MaxLength(256)]
        public string OrganisationName { get; set; }

        public bool IsActive { get; set; }
    }
}
