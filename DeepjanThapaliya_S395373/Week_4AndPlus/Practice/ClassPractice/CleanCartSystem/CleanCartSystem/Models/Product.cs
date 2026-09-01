using CleanCartSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCartSystem.Models
{
    internal class Product:IEntity
    {
        public int Id { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }

        public Product(int id, string productName, decimal price)
        {
            if(id < 1)
            {
                throw new ArgumentException("Product id must be greater than zero."); ;
            }

            if(price < 0)
            {
                throw new ArgumentException("Product Error: Price cannot be negative.");
            }
            Id = id;
            ProductName = productName ?? throw new ArgumentException("Product Error: Product name cannot be empty.");

            Price = price;
        }

        public override string ToString()
        {
            return $"Product Id: {Id} - Name: {ProductName} - " +
                $"Price: {Price}";
        }
    }
}
