namespace Basket.API.Models
{
    public class BuyerBasket
    {
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new();

        public BuyerBasket()
        {
            
        }

        public BuyerBasket(string buyerId)
        {
            BuyerId = buyerId;
        }
    }
}
