namespace ShoppingCartAPI.Models
{
    public class ShoppingCartItem
    {
        public int Quantity { get; set; }
        public string Color { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public required string ProductId { get; set; } 
        public required string ProductName { get; set; } 
    }
}