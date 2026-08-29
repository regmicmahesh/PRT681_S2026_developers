using FinalLINQPractical.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalLINQPractical.Models
{
    internal class Product: IEntity
    {
        public int Id { get; private set; }
        public string ProductName { get; private set; }
        public string Category { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }

        public Product(int id, string productName, string category, decimal price, int stockQuantity)
        {
            Id = id;
            ProductName = productName;
            Category = category;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public override string ToString()
        {
            return $"{Id} - {ProductName} - {Category} - ${Price} - Stock: {StockQuantity}";
        }
    }
}
