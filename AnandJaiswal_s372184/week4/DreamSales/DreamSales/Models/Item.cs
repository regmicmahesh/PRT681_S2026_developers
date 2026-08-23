using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamSales.Models

{

    [Table("Item")]
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required.")]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.00")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }

        // Seller ID to link the item to the seller (foreign key to ApplicationUser)
        public string SellerId { get; set; }
    }
}
