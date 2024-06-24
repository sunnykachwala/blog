namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class PollQuestion
    {
        [Key]
        [Required]
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public int PollId { get; set; }

        public Poll Poll { get; set; }
        public ICollection<PollAnswer> PollAnswer { get; set; }
    }
}
