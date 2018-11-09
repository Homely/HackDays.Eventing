namespace PublisherManual.Events
{
    public class ListingCreatedOrUpdatedEvent : ListingEvent
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public EventType EventType { get; set; }
    }
}