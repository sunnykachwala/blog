namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;

    public interface IAudited
    {
        public DateTime CreatedAt { get; set; }
        [MaxLength(256)]
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        [MaxLength(256)]
        public string? ModifiedBy { get; set; }
    }
}
