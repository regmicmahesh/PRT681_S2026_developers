namespace DreamSales.Models
{
    public interface ICartService
    {
        // Add an item to the cart with a specified quantity
        void AddItem(Item item, int quantity);

        // Remove a specific quantity of an item from the cart, returns the remaining quantity
        int RemoveItem(int itemId, int quantity);

        // Get the total quantity of a specific item in the cart
        int TotalItem(int itemId);

        // Get the total number of items in the cart
        int CartCount();

        // Clear all items from the cart
        void ClearCart();

        // Retrieve the current cart instance
        Cart GetCart();
    }
}
