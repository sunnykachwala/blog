namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    public class SubscriberAnswer
    {
        [Key]
        [Required]
        public Guid SubscriberAnswerId { get; set; }
        public Guid SubscriberId { get; set; }
        public Guid PollAnswerId { get; set; }

        public Subscriber Subscriber { get; set; }
        public PollAnswer PollAnswer { get; set; }
    }
}
