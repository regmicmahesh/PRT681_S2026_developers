namespace DreamSales.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        // Navigation property to the associated Item
        public virtual Item Item { get; set; }

        // Quantity of the item in the cart
        public int Quantity { get; set; }

        // Navigation property to the associated Cart
        public virtual Cart Cart { get; set; }

        // Price should be a decimal for better handling of monetary values
        public decimal Price { get; set; }

        // UserId should match the type of the user identifier in ASP.NET Identity (likely string)
        public string UserId { get; set; }
    }
}
