namespace Np.DAL.Domain
{
    public class Subscriber
    {
        public int SubscriberId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public bool IsSubscribed { get; set; }

        public string IpAddress { get; set; }

        public ICollection<SubscriberAnswer> SubscriberAnswer { get; set; }
    }
}
