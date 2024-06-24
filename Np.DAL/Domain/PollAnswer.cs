namespace Np.DAL.Domain
{
    public class PollAnswer
    {
        public int AnswerId { get; set; }
        public string Text { get; set; }
        public int QuestionId { get; set; }

        public PollQuestion Question { get; set; }
        public ICollection<SubscriberAnswer> UserAnswers { get; set; }
    }
}
