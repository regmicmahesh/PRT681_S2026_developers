using LINQFirstOrDefault.Models;

List<Product> products = new List<Product>();

products.Add(new Product(1, "Laptop", 1500m));
products.Add(new Product(2, "Mouse", 25m));
products.Add(new Product(3, "Keyboard", 80m));

Product? foundProduct = products.FirstOrDefault(product => product.Id ==2);//lambda expression
if (foundProduct == null)
{
    Console.WriteLine("Product not found.");
}
else
{
    Console.WriteLine($"\nProduct Found: {foundProduct}\n");
}

