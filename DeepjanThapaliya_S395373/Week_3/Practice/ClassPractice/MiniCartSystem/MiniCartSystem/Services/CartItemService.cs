using MiniCartSystem.Models;
using MiniCartSystem.Repositries;
using MiniCartSystem.Calculators;
using System;
using System.Collections.Generic;
using System.Text;
using MiniCartSystem.Interfaces;


namespace MiniCartSystem.Services
{
    internal class CartItemService
    {
        private InMemoryRepository<Customer> _customerRepo;
        private InMemoryRepository<Product> _productRepo;
        private IDiscountCalculator _discountCalculator;
      

        public CartItemService(InMemoryRepository<Customer> customerRepo, InMemoryRepository<Product> productRepo, IDiscountCalculator simpleDiscountCalculator)
        {
            if (customerRepo == null)
            {
                throw new ArgumentNullException("Cart Service Error: Customer doesnt exist.");
            }
            if (productRepo == null)
            {
                throw new ArgumentNullException("Cart Service Error: Product doesnt exist.");
            }
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            
            _discountCalculator = simpleDiscountCalculator;
        }

        
        public void CalculateTotal(int customerId, int productId, decimal quantity)
        {
            if( quantity < 1)
            {
                throw new ArgumentException("Cart Service Error: Quantity cannot be less than 1.");
            }
            Customer? customer = _customerRepo.GetById(customerId);
            Product? item = _productRepo.GetById(productId);

            if (customer == null)
            {
                throw new ArgumentException("Customer not found.");
            }

            if (item == null)
            {
                throw new ArgumentException("Product not found.");
            }

            Console.WriteLine(customer);
            Console.WriteLine(item);
            Console.WriteLine();
            decimal subtotal = new CartItem(item, quantity).GetLineTotal();
            Console.WriteLine($"Subtotal: {subtotal}");
            Console.WriteLine();

            decimal discount = _discountCalculator.CalculateDiscount(customer, subtotal);
            Console.WriteLine($"Discount: {discount}");
            Console.WriteLine();

            decimal finalPrice = subtotal - discount;
            Console.WriteLine($"Total: {finalPrice}");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine();

        }
    }
}
