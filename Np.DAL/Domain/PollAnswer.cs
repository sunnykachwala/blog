using System.ComponentModel.DataAnnotations;

namespace Np.DAL.Domain
{
    public class PollAnswer
    {
        [Key]
        [Required]
        public Guid AnswerId { get; set; }
      
        [Required]
        [MaxLength(512)]
        public string Text { get; set; }
        public Guid QuestionId { get; set; }

        public PollQuestion Question { get; set; }
        public ICollection<SubscriberAnswer> SubscriberAnswer { get; set; }
    }
}
