using MiniCartSystem.Calculators;
using MiniCartSystem.Models;
using MiniCartSystem.Repositries;
using MiniCartSystem.Services;

try
{
    InMemoryRepository<Customer> customerRepository = new InMemoryRepository<Customer>();
    InMemoryRepository<Product> productRepository = new InMemoryRepository<Product>();
    SimpleDiscountCalculator simpleDiscountCalculator = new SimpleDiscountCalculator();


    customerRepository.Add(new Customer(1, "Deepjan Thapaliya", true));
    customerRepository.Add(new Customer(2, "Alice Spring", false));
    customerRepository.Add(new Customer(3, "Alice Spring", true));
    customerRepository.Add(new Customer(4, "North Territory", false));
    Console.WriteLine();

    productRepository.Add(new Product(1, "Laptop", 1500m));
    productRepository.Add(new Product(2, "Mouse", 25m));
    productRepository.Add(new Product(3, "Keyboard", 80m));
    Console.WriteLine();

    customerRepository.PrintAll();
    customerRepository.RemoveById(3);
    customerRepository.RemoveById(4);
    customerRepository.PrintAll();
    Console.WriteLine();

    productRepository.PrintAll();
    productRepository.RemoveById(3);
    productRepository.PrintAll();
    Console.WriteLine();

    CartItemService cartItemService = new CartItemService(customerRepository, productRepository, simpleDiscountCalculator);
    cartItemService.CalculateTotal(1, 1, 2m);
    cartItemService.CalculateTotal(2, 1, 2m);
    cartItemService.CalculateTotal(1, 2, 5m);
    cartItemService.CalculateTotal(2, 2, 5m);
    cartItemService.CalculateTotal(2, 3, 4m);
    Console.ReadLine();
} catch(Exception ex)
{
    Console.WriteLine($"{ex.Message}");
}