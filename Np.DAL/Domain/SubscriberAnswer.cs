namespace Np.DAL.Domain
{
    public class SubscriberAnswer
    {
        public int SubscriberAnswerId { get; set; }
        public int SubscriberId { get; set; }
        public int AnswerId { get; set; }

        public Subscriber Subscriber { get; set; }
        public PollAnswer PollAnswer { get; set; }
    }
}
