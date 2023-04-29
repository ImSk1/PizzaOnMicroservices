namespace Menu.API.Events
{
    public class PizzaCreatedEvent : IntegrationEvent
    { 
        public string Name { get; set; }
    }
}
