using CleanCartSystem.Interfaces;
using CleanCartSystem.Models;
using CleanCartSystem.Repositories;
using CleanCartSystem.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCartSystem.Services
{
    internal class CartService
    {
        private readonly InMemoryRepository<Customer> _customerRepository;
        private readonly InMemoryRepository<Product> _productRepository;
        private readonly IDiscountCalculator _discountCalculator;
        private readonly Dictionary<int, List<CartItem>> _customerCarts = new Dictionary<int, List<CartItem>>();

        internal CartService(InMemoryRepository<Customer> customerRepository,
            InMemoryRepository<Product> productRepository, IDiscountCalculator discountCalculator)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _discountCalculator = discountCalculator ?? throw new ArgumentNullException(nameof(discountCalculator));
        }
        

        public Result<CartItem> AddItemToCart (int customerId,int productId, int quantity)
        {
            if (customerId < 1)
            {
                return Result<CartItem>.Failure("Customer  id must be greater than zero.");
            }

            if (productId < 1)
            {
                return Result<CartItem>.Failure("Product id must be greater than zero.");
            }

            if (quantity < 1)
            {
                return Result<CartItem>.Failure("Quantity must be more than zero.");

            }

            Customer? customer = _customerRepository.GetById(customerId);
            if (customer == null)
            {
                return Result<CartItem>.Failure("Customer not found");

            }

            Product? product = _productRepository.GetById(productId);
            if(product == null)
            {
                return Result<CartItem>.Failure("Product not found.");
            }
            
            if (!_customerCarts.ContainsKey(customerId))
            {
                _customerCarts[customerId] = new List<CartItem>();
            }
            List<CartItem> customerCart = _customerCarts[customerId];

            CartItem? existingCartItem = customerCart.FirstOrDefault(item => item.Product.Id == productId);
            if(existingCartItem != null)
            {
                existingCartItem.IncreaseQuantity(quantity);
                return Result<CartItem>.Success(existingCartItem, "Product quantity updated successfully.");
            }
            CartItem cartItem = new CartItem(product, quantity);
            customerCart.Add(cartItem);

            return Result<CartItem>.Success(cartItem, "Product Added to cart successfully.");
        }

        public Result<CartSummary> GetCartSummary(int customerId)
        {
            if(customerId < 1)
            {
                return Result<CartSummary>.Failure("Customer Id must be greater" +
                    " than zero.");
            }

            Customer? customer = _customerRepository.GetById(customerId);
            if(customer == null)
            {
                return Result<CartSummary>.Failure("Customer is not found.");
            }

            if (!_customerCarts.ContainsKey(customerId) || !_customerCarts[customerId].Any())
            {
                return Result<CartSummary>.Failure("Cart is empty.");

            }

            decimal subtotal = _customerCarts[customerId].Sum(item => item.GetLineTotal());
            decimal discount = _discountCalculator.GetDiscount(customer, subtotal);
            decimal finalTotal = subtotal - discount;

            CartSummary summary = new CartSummary(
                customer,
                _customerCarts[customerId].ToList(),
                subtotal,
                discount,
                finalTotal
                );
            return Result<CartSummary>.Success(summary, "Cart summary created.");

        }

        public Result<CartItem> RemoveItemFromCart(int customerId, int productId)
        {
            if (customerId < 1)
            {
                return Result<CartItem>.Failure("Customer  id must be greater than zero.");
            }

            if (productId < 1)
            {
                return Result<CartItem>.Failure("Product id must be greater than zero.");
            }

            Customer? customer = _customerRepository.GetById(customerId);

            if (customer == null)
            {
                return Result<CartItem>.Failure("Customer not found.");
            }

            List<CartItem> customerCart = _customerCarts[customerId];

            CartItem? existingCartItem = customerCart.FirstOrDefault(item => item.Product.Id == productId);

            if(existingCartItem == null)
            {
                return Result<CartItem>.Failure($"{nameof(Result<CartItem>)}There is no product on the cart to remove");
            }

            customerCart.Remove(existingCartItem);

            return Result<CartItem>.Success(existingCartItem, "Product removed from cart succesfully");
            


        }

        public Result<CartItem> DecreaseProductQuantity(int customerId, int productId)
        {
            if (customerId < 1)
            {
                return Result<CartItem>.Failure("Customer  id must be greater than zero.");
            }

            if (productId < 1)
            {
                return Result<CartItem>.Failure("Product id must be greater than zero.");
            }

            Customer? customer = _customerRepository.GetById(customerId);
            if (customer == null)
            {
                return Result<CartItem>.Failure("Customer not found");

            }

            Product? product = _productRepository.GetById(productId);
            if (product == null)
            {
                return Result<CartItem>.Failure("Product not found.");
            }
            if (!_customerCarts.ContainsKey(customerId))
            {
                return Result<CartItem>.Failure("Customer cart not found");
            }
            List<CartItem> existingCartItems = _customerCarts[customerId];

            CartItem? cartItem = existingCartItems.FirstOrDefault(item => item.Product.Id == productId);

            if (cartItem == null)
            {
                return Result<CartItem>.Failure("Product not found");
            }
            cartItem.DecreaseQuantity();
            return Result<CartItem>.Success(cartItem, " product Quantity decrease successfully");
        }
    }


    
}
