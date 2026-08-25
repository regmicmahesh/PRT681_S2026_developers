using LINQFirstOrDefault.Models;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

List<Product> products = new List<Product>
{
    new Product(1, "Laptop", "Electronics", 1500m, 5),
    new Product(2, "Mouse", "Electronics", 25m, 50),
    new Product(3, "Keyboard", "Electronics", 80m, 30),
    new Product(4, "Office Chair", "Furniture", 250m, 10),
    new Product(5, "Desk", "Furniture", 400m, 7),
    new Product(6, "Notebook", "Stationery", 5m, 100),
    new Product(7, "Pen", "Stationery", 2m, 200)
};

Product? foundProduct = products.FirstOrDefault(product => product.Id ==2);//lambda expression. Returns first true.
if (foundProduct == null)
{
    Console.WriteLine("Product not found.");
}
else
{
    Console.WriteLine($"\nProduct Found: {foundProduct}\n");
}

List<Product> expensiveProducts = products.Where(product => product.Price >= 100m).ToList();


Console.WriteLine("Expensive products:");
Console.WriteLine();

foreach (Product product in expensiveProducts)
{
    Console.WriteLine(product);
}


//Where() = filter many items
//FirstOrDefault() = find one item
//OrderBy() = sort items
//Select() = transform items
//Count() = count items
//Sum() = total values
//Any() = check if at least one exists
//foreach = do an action for each item