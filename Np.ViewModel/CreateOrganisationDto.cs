namespace Np.ViewModel
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOrganisationDto
    {
        [Required]
        [MaxLength(256)]
        public string OrganisationName { get; set; }

        public bool IsActive { get; set; }
    }
}
