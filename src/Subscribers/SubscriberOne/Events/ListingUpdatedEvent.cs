namespace SubscriberOne.Events
{
    public class ListingUpdatedEvent
    {
        public int ListingId { get; set; }
        public int Price { get; set; }
    }
}