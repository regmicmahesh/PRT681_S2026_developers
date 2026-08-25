
using System;
using System.Collections.Generic;
using System.Text;

namespace LINQFirstOrDefault.Models
{
    public class Product
    {
        public int Id { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }

        public Product(int id, string productName, decimal price)
        {
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
