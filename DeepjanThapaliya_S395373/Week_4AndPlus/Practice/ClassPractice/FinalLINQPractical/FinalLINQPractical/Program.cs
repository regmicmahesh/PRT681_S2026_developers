using FinalLINQPractical.Models;
using FinalLINQPractical.Repositories;

InMemoryRepository<Product> productRepository = new InMemoryRepository<Product>();

productRepository.Add(new Product(1, "Laptop", "Electronics", 1500m, 5));
productRepository.Add(new Product(2, "Mouse", "Electronics", 25m, 50));
productRepository.Add(new Product(3, "Keyboard", "Electronics", 80m, 10));
productRepository.Add(new Product(4, "Monitor", "Electronics", 300m, 0));

Product? foundProduct = productRepository.GetById(2);

Console.WriteLine(foundProduct == null ? "Product not found." : foundProduct);

Console.WriteLine($"Does product 3 exist? {productRepository.Exists(3)}");
Console.WriteLine($"Does product 99 exist? {productRepository.Exists(99)}");
Console.WriteLine($"Total products: {productRepository.Count()}");

bool removed = productRepository.RemoveById(2);

Console.WriteLine(removed ? "Product removed." : "Product not found.");
Console.WriteLine($"Total products after remove: {productRepository.Count()}");