using MiniCartSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCartSystem.Models
{
    internal class Product:IEntity
    {
        public int Id { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }

        public Product(int id, string name, decimal price)
        {
            if(id < 1)
            {
                throw new ArgumentException("Id cannot be less than 1.");
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name cannot be empty.");
            }
            if (price < 0)
            {
                throw new ArgumentException("Products price cannot be negative.");
            }
            Id = id;
            ProductName = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"Product id: {Id}, Name: {ProductName}, Price: {Price}";
        }
    }
}
