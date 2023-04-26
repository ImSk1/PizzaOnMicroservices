namespace Basket.API.Models
{
    public class BasketItem
    {
        public string Id { get; set; }
        public string PizzaId { get; set; }
        public string PizzaName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
