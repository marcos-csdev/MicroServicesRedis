namespace ShoppingCartAPI.Models
{
    public class ShoppingCart
    {
        public string UserName { get; set; } = string.Empty;
        public List<ShoppingCartItem> Items { get; set; } = null!;

        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }

        public ShoppingCart(string userName)
        {
            Items = new List<ShoppingCartItem>();
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                for (int i = 0; i < Items.Count; i++)
                {
                    totalprice += Items[i].Price * Items[i].Quantity;
                }
                return totalprice;
            }
        }
    }
}
