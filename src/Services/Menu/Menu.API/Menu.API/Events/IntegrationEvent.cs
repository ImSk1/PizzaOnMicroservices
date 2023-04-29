namespace Menu.API.Events
{
    public abstract class IntegrationEvent
    {
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}
