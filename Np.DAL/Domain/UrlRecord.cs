namespace Np.DAL.Domain
{
    using Np.Common;
    using System.ComponentModel.DataAnnotations;
    public class UrlRecord
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(160)]
        public string Slug { get; set; }
        public UrlEntityType EntityType { get; set; }
        public Guid EntityId { get; set; }

        public bool IsActive { get; set; }
    }
}
