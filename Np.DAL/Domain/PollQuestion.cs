namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class PollQuestion
    {
        [Key]
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        [MaxLength(512)]
        public string Text { get; set; }
        public Guid PollId { get; set; }

        public Poll Poll { get; set; }
        public ICollection<PollAnswer> PollAnswer { get; set; }
    }
}
