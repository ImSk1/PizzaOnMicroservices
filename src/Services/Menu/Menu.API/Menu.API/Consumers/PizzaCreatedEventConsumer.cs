using MassTransit;
using Menu.API.Events;

namespace Menu.API.Consumers
{
    public class PizzaCreatedEventConsumer : IConsumer<PizzaCreatedEvent>
    {
        public async Task Consume(ConsumeContext<PizzaCreatedEvent> context)
        {
            Console.Beep();
            Console.WriteLine("test");
        }
    }
}
