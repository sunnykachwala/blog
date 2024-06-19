namespace Np.ViewModel
{
    using System.ComponentModel.DataAnnotations;
    public class CreateUrlReordDto
    {
        [Required]
        [MaxLength(160)]
        public string Slug { get; set; }
        public byte EntityType { get; set; }
        public Guid EntityId { get; set; }

        public bool IsActive { get; set; }
    }
}
