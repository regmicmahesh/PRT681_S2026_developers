using ResultPatternPractice.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResultPatternPractice.Models
{
    internal class Product: IEntity
    {
        public int Id { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }

        public Product(int id, string productName, decimal price)
        {
            if (id < 1)
            {
                throw new ArgumentException("Product id must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }

            if (price < 0)
            {
                throw new ArgumentException("Product price cannot be negative.");
            }

            Id = id;
            ProductName = productName;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Id} - {ProductName} - ${Price}";
        }
    }
}