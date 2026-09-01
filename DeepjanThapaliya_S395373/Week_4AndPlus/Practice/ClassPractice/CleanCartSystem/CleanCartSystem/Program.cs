using CleanCartSystem.Calculators;
using CleanCartSystem.Common;
using CleanCartSystem.Interfaces;
using CleanCartSystem.Models;
using CleanCartSystem.Repositories;
using CleanCartSystem.Services;

try
{
    InMemoryRepository<Customer> customerRepository = new InMemoryRepository<Customer>();
    InMemoryRepository<Product> productRepository = new InMemoryRepository<Product>();

    productRepository.Add(new Product(1, "Laptop", 1500m));
    productRepository.Add(new Product(2, "Mouse", 25m));
    productRepository.Add(new Product(3, "Keyboard", 80m));
    productRepository.Add(new Product(4, "Monitor", 300m));

    customerRepository.Add(new Customer(1, "Deepjan Thapaliya", true));
    customerRepository.Add(new Customer(2, "Alex Smith", false));

    CartService cartService = new CartService(customerRepository, productRepository, new SimpleDiscountCalculator());

    Result<CartItem> result1 = cartService.AddItemToCart(1,1, 1);
    Console.WriteLine(result1.Message);

    Result<CartItem> result2 = cartService.AddItemToCart(1,2, 2);
    Console.WriteLine(result2.Message);

    Result<CartItem> result3 = cartService.AddItemToCart(2, 3, 1);
    Console.WriteLine(result3.Message);

    Result<CartItem> result4 = cartService.AddItemToCart(2,99, 1);
    Console.WriteLine(result4.Message);

    Result<CartItem> result5 = cartService.AddItemToCart(1, 2, 2);
    Console.WriteLine(result5.Message);

    Console.WriteLine();


    Result<CartSummary> result = cartService.GetCartSummary(1);
    PrintCartSummary(result);

    Console.WriteLine();

    Result<CartSummary> resultCustomer2 = cartService.GetCartSummary(2);
    PrintCartSummary(resultCustomer2);


    static void PrintCartSummary(Result<CartSummary> result)
    {
        Console.WriteLine(result.Message);

        if (!result.IsSuccess || result.Data == null)
        {
            return;
        }

        CartSummary cartSummary = result.Data;

        Console.WriteLine();
        Console.WriteLine("Cart Summary:");
        Console.WriteLine(cartSummary.Customer);
        Console.WriteLine("----------------------------------------------------------------");
        Console.WriteLine();

        foreach (CartItem item in cartSummary.Items)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine();

        Console.WriteLine($"Subtotal: ${cartSummary.Subtotal}");
        Console.WriteLine($"Discount: ${cartSummary.Discount}");
        Console.WriteLine($"Final Total: ${cartSummary.FinalTotal}");
    }
}
catch (ArgumentException ex)
{
    Console.WriteLine(ex.Message);
}
