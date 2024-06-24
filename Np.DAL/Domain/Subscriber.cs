namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class Subscriber
    {
        [Key]
        [Required]
        public Guid SubscriberId { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        public bool IsSubscribed { get; set; }

        public string IpAddress { get; set; }

        public ICollection<SubscriberAnswer> SubscriberAnswer { get; set; }
    }
}
