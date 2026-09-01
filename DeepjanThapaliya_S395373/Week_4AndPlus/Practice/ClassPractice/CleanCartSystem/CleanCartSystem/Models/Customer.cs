using CleanCartSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCartSystem.Models
{
    internal class Customer:IEntity
    {
        public int Id { get; private set; }
        public string CustomerName { get; private set; }
        public bool IsPremiumCustomer { get; private set; }

        public Customer(int id, string customerName, bool isPremiumCustomer)
        {
            if (id < 1)
            {
                throw new ArgumentException("Customer Error: Customer Id needs to be a natural number.");
            }

            

            Id = id;
            CustomerName = customerName ?? throw new ArgumentException("Customer Error: Customer name cannot be Empty.");
            IsPremiumCustomer = isPremiumCustomer;
        }

        public override string ToString()
        {
            string status = IsPremiumCustomer ? "Premium" : "Normal";
            return $"Customer id: {Id} - Name: {CustomerName} - Status: {status}";
        }
    }
}
