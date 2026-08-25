using MiniCartSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCartSystem.Models
{
    internal class Customer:IEntity
    {
        public int Id { get; private set; }
        public string FullName { get; private set; }

        public bool IsPremium { get; private set; }

        public Customer(int id, string name, bool isPremium)
        {
            if(id <= 0)
            {
                throw new ArgumentException("Customer Id cannot be less than 1.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Customers name cannot be empty.");
            }
            Id = id;
            FullName = name;
            IsPremium = isPremium;

            
        }

        public override string ToString()
        {
            if (IsPremium)
            {
                return $"Customer id: {Id}, Name: {FullName}, Status: Premium";
            }
            else
            {
                return $"Customer id: {Id}, Name: {FullName}, Status: Non-Premium";
            }
        }
    }
}
